using Anomoly.KitsPlus.Data;
using Newtonsoft.Json;
using Rocket.API;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Databases
{
    public class JsonDatabase : IKitDatabase
    {
        private List<Kit> _kits;

        private string _file;


        public string Name => "Json";


        public JsonDatabase()
        {
            var directory = KitsPlusPlugin.Instance.Directory;
            _file = Path.Combine(directory, "kits_data.json");

            Logger.Log("Initializing JsonDatabase...");
            LoadOrCreateJsonFile();
        }

        private void LoadOrCreateJsonFile()
        {
            if (!File.Exists(_file))
            {
                var defaultState = new List<Kit>()
                {
                    new Kit()
                    {
                        Name ="Survival",
                        XP = null,
                        Vehicle = null,
                        Cooldown = 300,
                        Items = new List<KitItem>()
                        {
                            new KitItem()
                            {
                                Id = 251,
                                Amount = 1,
                            },
                            new KitItem()
                            {
                                Id = 81,
                                Amount = 2,
                            },
                            new KitItem()
                            {
                                Id = 16,
                                Amount = 1,
                            }
                        }
                    }
                };
                File.WriteAllText(_file, JsonConvert.SerializeObject(defaultState, Formatting.Indented));
                _kits = defaultState;
                return;
            }

            var json = File.ReadAllText(_file);

            _kits = JsonConvert.DeserializeObject<List<Kit>>(json);
        }

        private void SaveJsonFile()
        {
            var json = JsonConvert.SerializeObject(_kits, Formatting.Indented);
            File.WriteAllText(_file, json);
        }

        public bool CreateKit(Kit kit)
        {
            _kits.Add(kit);
            SaveJsonFile();
            return true;
        }

        public int DeleteKit(string name)
        {
            var deleted = _kits.RemoveAll(k => k.Name.ToLower() == name.ToLower());
            SaveJsonFile();
            return deleted;
        }

        public Kit GetKitByName(string name)
            => _kits.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        public Kit GetKitByName(IRocketPlayer player, string name)
            => GetKits(player).FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        public List<Kit> GetKits()
            => _kits;

        public List<Kit> GetKits(IRocketPlayer player)
            => _kits.Where(k => player.HasPermission($"kit.${k.Name}")).ToList();

        public void Dispose()
        {
            SaveJsonFile();
        }
    }
}
