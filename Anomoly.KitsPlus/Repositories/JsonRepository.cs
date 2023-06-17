using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Utils;
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
    public class JsonRepository : IKitRepository
    {
        private JsonFileDb<List<Kit>> _kits;

        private string _file;

        public string Name => "Json";

        public bool CreateKit(Kit kit)
        {
            var existingKit = GetKitByName(kit.Name);
            if (existingKit != null)
                return false;

            _kits.Instance.Add(kit);
            _kits.Save();
            return true;
        }

        public int DeleteKit(string name)
        {
            var deleted = _kits.Instance.RemoveAll(k => k.Name.ToLower() == name.ToLower());
            
            if(deleted > 0)
                _kits.Save();

            return deleted;
        }

        public Kit GetKitByName(string name)
            => _kits.Instance.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        public Kit GetKitByName(IRocketPlayer player, string name)
            => GetKits(player).FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        public List<Kit> GetKits()
            => _kits.Instance;
        public List<Kit> GetKits(IRocketPlayer player)
            => GetKits().Where(k => player.HasPermission($"kit.{k.Name}")).ToList();

        public void Dispose()
        {
            Logger.Log("Saving kits....");
            _kits = null;
        }

        public void Initialize(Kit[] defaultKits)
        {
            var directory = KitsPlusPlugin.Instance.Directory;
            _file = Path.Combine(directory, "kits_data.json");

            _kits = new JsonFileDb<List<Kit>>(_file, defaultKits.ToList(), Formatting.Indented);

            Logger.Log("Initializing JsonDatabase...");
            _kits.Load();
        }

        public void Reset()
        {
            _kits.Instance.Clear();
            _kits.Save();
        }
    }
}
