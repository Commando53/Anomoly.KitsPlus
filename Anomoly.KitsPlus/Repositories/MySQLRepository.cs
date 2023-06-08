using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Databases.Migrations;
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
    public class MySQLRepository : IKitRepository
    {
        public string Name => "MySQL";

        private DbConnection dbConnection;
        private readonly IMySQLMigration[] _migrations = new IMySQLMigration[] { new MaxUsage_Migration() };

        public const string KITS_TABLE = "kitsplus_kits";
        public const string KIT_ITEMS_TABLE = "kitsplus_kit_items";
        public const string MIGRATION_TABLE = "kitsplus_migrations";

        private static string _tablePrefix => KitsPlusPlugin.Instance.Configuration.Instance.MySQLTablePrefix;

        public static string KitsTable => $"{_tablePrefix}_{KITS_TABLE}";
        public static string KitItemsTable => $"{_tablePrefix}_{KIT_ITEMS_TABLE}";
        public static string MigrationsTable => $"{_tablePrefix}_{MIGRATION_TABLE}";

        private void CheckSchema()
        {
            try
            {
                var kitsTableCreated = dbConnection.ExecuteUpdate($"CREATE TABLE IF NOT EXISTS `{KitsTable}` (`name` varchar(32) NOT NULL, `vehicle` smallint unsigned, `xp` int unsigned, `cooldown` int, PRIMARY KEY (`name`));");
                if (kitsTableCreated > 0)
                    Logger.Log($"Successfully created table {KitsTable}");
                var itemsTableCreated = dbConnection.ExecuteUpdate($"CREATE TABLE IF NOT EXISTS `{KitItemsTable}` (`kit_name` varchar(32) NOT NULL, `id` smallint unsigned, `amount` tinyint unsigned, PRIMARY KEY(`kit_name`,`id`))");
                if (itemsTableCreated > 0)
                    Logger.Log($"Successfully created table {KitItemsTable}");
                var migrationTableCreated = dbConnection.ExecuteUpdate($"CREATE TABLE IF NOT EXISTS `{MigrationsTable}` (`id` int NOT NULL AUTO_INCREMENT, `name` varchar(155) NOT NULL, PRIMARY KEY(`id`));");
                if (migrationTableCreated > 0)
                    Logger.Log($"Successfully created table {MigrationsTable}");
            }
            catch(Exception ex)
            {
                Logger.LogWarning("Unable to verify if table schemas were created.");
                Logger.LogException(ex, "Error verifying table schema");
            }

            CheckMigrations();
        }

        private bool MigrationExists(string name)
        {
            bool exists = false;
            try
            {
                var reader = dbConnection.Execute($"SELECT COUNT(`id`) AS `FoundMigrations` FROM `{MigrationsTable}`;");
                if (reader.HasRows && reader.Read())
                {
                    var count = reader.GetInt32("FoundMigrations");
                    exists = count > 0;
                }
                reader.Close();
            }
            catch(Exception ex) { Logger.LogException(ex, $"Failed to check if migration exists: {name}"); }

            return exists;
        }

        private void CheckMigrations()
        {
            foreach(var migration in _migrations)
            {
                if (MigrationExists(migration.Name))
                    continue;

                Logger.Log($"New migration found: {migration.Name}");
               
                int created = dbConnection.ExecuteUpdate($"INSERT INTO `{MigrationsTable}` (`name`) VALUES(?);", migration.Name);
                if (created > 0)
                    migration.Up(dbConnection);
                else
                    Logger.LogError($"Failed to migration: {migration.Name}");
            }
        }

        public bool CreateKit(Kit kit)
        {
            try
            {
                int affectedRows = dbConnection.ExecuteUpdate($"INSERT INTO `{KitsTable}` (`name`, `vehicle`, `xp`, `cooldown`, `maxusage`) VALUES(?,?,?,?,?);", kit.Name, kit.Vehicle, kit.XP, kit.Cooldown, kit.MaxUsage);

                if (affectedRows > 0)
                {
                    foreach (var item in kit.Items)
                    {
                        dbConnection.ExecuteUpdate($"INSERT INTO `{KitItemsTable}` (`kit_name`, `id`, `amount`) VALUES(?,?,?);", kit.Name, item.Id, item.Amount);
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                Logger.LogWarning("Failed to create kit.");
                Logger.LogException(ex, "Error on creating kit");
            }

            return false;
        }

        public int DeleteKit(string name)
        {
            dbConnection.ExecuteUpdate($"DELETE FROM `{KitItemsTable}` WHERE `kit_name` = ?", name);
            int deleted = dbConnection.ExecuteUpdate($"DELETE FROM `{KitsTable}` WHERE `name` = ?;", name);
            return deleted;
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
                    kit.MaxUsage = reader.GetInt32("maxusage");
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
            catch(Exception ex) { Logger.LogException(ex, "Error getting kit."); }
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
                            MaxUsage = reader.GetInt32("maxusage"),
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


        public void Dispose()
        {
            dbConnection.Dispose();
        }

        public void Initialize(Kit[] defaultKits)
        {
            dbConnection = new DbConnection(KitsPlusPlugin.Instance.Configuration.Instance.MySQLConnectionString);
            Logger.Log("Initializing MySQLDatabase...");
            CheckSchema();
        }

        public void Reset()
        {
            dbConnection.ExecuteUpdate($"DROP TABLE `{KitItemsTable}`;");
            dbConnection.ExecuteUpdate($"DROP TABLE `{KitsTable}`;");
            dbConnection.ExecuteUpdate($"DROP TABLE `{MigrationsTable}`;");

            CheckSchema();
        }
    }
}
