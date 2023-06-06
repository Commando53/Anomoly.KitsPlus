using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Utils;
using Rocket.API;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Databases
{
    public class MySQLDatabase : IKitDatabase
    {
        public string Name => "MySQL";

        private DbConnection dbConnection;

        public const string KITS_TABLE = "kitsplus_kits";
        public const string KIT_ITEMS_TABLE = "kitsplus_kit_items";

        public string KitsTable => $"{KitsPlusPlugin.Instance.Configuration.Instance.MySQLTablePrefix}_{KITS_TABLE}";
        public string KitItemsTable => $"{KitsPlusPlugin.Instance.Configuration.Instance.MySQLTablePrefix}_{KIT_ITEMS_TABLE}";

        public MySQLDatabase()
        {
            dbConnection = new DbConnection(KitsPlusPlugin.Instance.Configuration.Instance.MySQLConnectionString);
            Logger.Log("Initializing MySQLDatabase...");
            CheckSchema();
        }

        private void CheckSchema()
        {
            dbConnection.ExecuteUpdate($"CREATE TABLE IF NOT EXISTS `{KitsTable}` (`name` varchar(32) NOT NULL, `vehicle` smallint unsigned, `xp` int unsigned, `cooldown` int, PRIMARY KEY (`name`));");
            dbConnection.ExecuteUpdate($"CREATE TABLE IF NOT EXISTS `{KitItemsTable}` (`kit_name` varchar(32) NOT NULL, `id` smallint unsigned, `amount` tinyint unsigned, PRIMARY KEY(`kit_name`,`id`))");
        }

        public bool CreateKit(Kit kit)
        {
            int affectedRows = dbConnection.ExecuteUpdate($"INSERT INTO `{KitsTable}` (`name`, `vehicle`, `xp`, `cooldown`) VALUES(?,?,?,?);", kit.Name, kit.Vehicle, kit.XP, kit.Cooldown);
            
            if(affectedRows > 0)
            {
                foreach (var item in kit.Items)
                {
                    dbConnection.ExecuteUpdate($"INSERT INTO `{KitItemsTable}` (`kit_name`, `id`, `amount`) VALUES(?,?,?);", kit.Name, item.Id, item.Amount);
                }

                return true;
            }

            return false;
        }

        public int DeleteKit(string name)
        {
            dbConnection.ExecuteUpdate($"DELETE FROM `{KitItemsTable}` WHERE `kit_name` = ?", name);
            return dbConnection.ExecuteUpdate($"DELETE FROM `{KitsTable}` WHERE `name` = ?;", name);
        }

        public Kit GetKitByName(string name)
        {
            Kit kit = null;
            try
            {
                var reader = dbConnection.Execute($"SELECT * FROM `{KitsTable}` WHERE `name` = ? LIMIT 1;", name);
                if (reader.HasRows && reader.Read())
                {
                    kit = new Kit();
                    kit.Name = reader.GetString("name");
                    kit.XP = reader.GetUInt32("xp");
                    kit.Vehicle = reader.GetUInt16("vehicle");
                    kit.Cooldown = reader.GetInt32("cooldown");
                    kit.Items = new List<KitItem>();

                    reader.Close();

                    var itemReader = dbConnection.Execute($"SELECT * FROM `{KitItemsTable}` WHERE `kit_name` = ?;", name);

                    if (itemReader.HasRows)
                    {
                        while (itemReader.Read())
                        {
                            kit.Items.Add(new KitItem()
                            {
                                Id = itemReader.GetUInt16("id"),
                                Amount = itemReader.GetByte("amount")
                            });
                        }
                    }
                    itemReader.Close();
                }
                else
                {
                    reader.Close();
                }
            }
            catch(Exception ex) { Logger.LogException(ex, "Failed to get kit by name."); }
            return kit;
        }

        public Kit GetKitByName(IRocketPlayer player, string name)
        {
            if (!player.HasPermission($"kit.{name}"))
            {
                return null;
            }

            var kit = GetKitByName(name);

            return kit;
        }

        public List<Kit> GetKits()
        {
            var list = new List<Kit>();
            try
            {
                var reader = dbConnection.Execute($"SELECT * FROM `{KitsTable}`");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new Kit()
                        {
                            Name = reader.GetString("name"),
                            XP = reader.GetUInt32("xp"),
                            Vehicle = reader.GetUInt16("vehicle"),
                            Cooldown = reader.GetInt32("cooldown"),
                            Items = new List<KitItem>(),
                        });
                    }
                }

                reader.Close();

                if(list.Count > 0)
                {
                    foreach(var kit in list)
                    {
                        var kitReader = dbConnection.Execute($"SELECT * FROM `{KitItemsTable}` WHERE `kit_name` = ?;", kit.Name);

                        if (kitReader.HasRows)
                        {
                            while (kitReader.Read())
                            {
                                kit.Items.Add(new KitItem()
                                {
                                    Id = kitReader.GetUInt16("id"),
                                    Amount = kitReader.GetByte("amount")
                                });
                            }
                        }

                        kitReader.Close();
                    }
                }
            }
            catch(Exception ex) { Logger.LogException(ex, "Failed to get kits."); }
            return list;
        }

        public List<Kit> GetKits(IRocketPlayer player)
        {
            return GetKits().Where(k => player.HasPermission($"kit.{k.Name}")).ToList();
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}
