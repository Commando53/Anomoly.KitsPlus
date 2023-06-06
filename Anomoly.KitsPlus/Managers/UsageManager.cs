using Anomoly.KitsPlus.Utils;
using Newtonsoft.Json;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Anomoly.KitsPlus.Managers
{
    public class UsageManager: IDisposable
    {
        private string _file;

        private JsonFileDb<Dictionary<string, int>> _usages;
        public UsageManager()
        {
            var directory = KitsPlusPlugin.Instance.Directory;
            _file = Path.Combine(directory, "kit_usages.json");
            Logger.Log("Initializing UsageManager...");

            _usages = new JsonFileDb<Dictionary<string, int>>(_file, new Dictionary<string, int>(), Formatting.None);
            _usages.Load();
        }

        public int GetKitUsage(string playerId, string kitName)
        {
            var key = $"{playerId}_{kitName}";
            if (!_usages.Instance.ContainsKey(key))
                return 0;

            return _usages.Instance[key];
        }

        public void AddUsage(string playerId, string kitName)
        {
            var key = $"{playerId}_{kitName}";
            if (_usages.Instance.ContainsKey(key))
                _usages.Instance[key]++;
            else
                _usages.Instance.Add(key, 1);

            _usages.Save();
        }

        public int DeleteAllUsages(string kitName)
        {
            var keys = _usages.Instance.Keys.Where(x => x.EndsWith($"_{kitName}")).ToList();

            int deleted = 0;
            foreach(var key in keys)
            {
                _usages.Instance.Remove(key);
                deleted++;
            }

            if (deleted > 0)
                _usages.Save();

            return deleted;
        }

        public void Dispose()
        {
            Logger.Log("Saving usages...");
            _usages.Save();
            _usages = null;
        }
    }
}
