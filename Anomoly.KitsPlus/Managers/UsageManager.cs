using Newtonsoft.Json;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Managers
{
    public class UsageManager
    {
        private Dictionary<string, int> _usages;

        private string _file;
        public UsageManager()
        {
            var directory = KitsPlusPlugin.Instance.Directory;
            _file = Path.Combine(directory, "kit_usages.json");
            Logger.Log("Initializing UsageManager...");
            CreateOrLoad();
        }

        private void CreateOrLoad()
        {
            if (!File.Exists(_file))
            {
                _usages = new Dictionary<string, int>();
                Save();
                return;
            }

            var json = File.ReadAllText(_file);

            _usages = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }

        public int GetKitUsage(string playerId, string kitName)
        {
            var key = $"{playerId}_{kitName}";
            if (!_usages.ContainsKey(key))
                return 0;

            return _usages[key];
        }

        public void AddUsage(string playerId, string kitName)
        {
            var key = $"{playerId}_{kitName}";
            if (_usages.ContainsKey(key))
                _usages[key]++;
            else
                _usages.Add(key, 1);

            Save();
        }

        public int DeleteAllUsages(string kitName)
        {
            var keys = _usages.Keys.Where(x => x.EndsWith($"_{kitName}")).ToList();

            int deleted = 0;
            foreach(var key in keys)
            {
                _usages.Remove(key);
                deleted++;
            }

            if (deleted > 0)
                Save();

            return deleted;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_usages);

            File.WriteAllText(_file, json);
        }
    }
}
