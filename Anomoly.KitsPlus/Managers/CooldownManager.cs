using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Utils;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Managers
{
    public class CooldownManager: IDisposable
    {
        private JsonFileDb<Dictionary<string, DateTime>> _global;
        private JsonFileDb<Dictionary<string, DateTime>> _kits;

        public int GlobalCooldown => KitsPlusPlugin.Instance.Configuration.Instance.GlobalCooldown;

        public CooldownManager()
        {
            Logger.Log("Initializing CooldownManager...");

            var directory = KitsPlusPlugin.Instance.Directory;
            var globalFile = Path.Combine(directory, "kit_cooldowns_g.json");
            var kitsFile = Path.Combine(directory, "kits_cooldowns_k.json");


            _global = new JsonFileDb<Dictionary<string, DateTime>>(globalFile, new Dictionary<string, DateTime>());
            _kits = new JsonFileDb<Dictionary<string, DateTime>>(kitsFile, new Dictionary<string, DateTime>());

            _global.Load();
            _kits.Load();
        }


        public int GetTimeLeftForGlobal(string playerId)
        {
            if (_global.Instance.ContainsKey(playerId))
            {
                var start = _global.Instance[playerId];

                var waitTime = (DateTime.Now - start).TotalSeconds;

                int timeLeft = (GlobalCooldown - Convert.ToInt32(waitTime));

                if (timeLeft <= 0)
                {
                    timeLeft = 0;
                    _global.Instance.Remove(playerId);
                }

                return timeLeft;
            }

            return 0;
        }

        public void SetGlobalCooldown(string playerId)
        {
            if (GlobalCooldown <= 0)
                return;

            if (_global.Instance.ContainsKey(playerId))
                _global.Instance[playerId] = DateTime.Now;
            else
                _global.Instance.Add(playerId, DateTime.Now);
        }

        public int GetTimeLeftForKit(IRocketPlayer player, string kitName)
        {
            var key = $"{player.Id}_{kitName}".ToLower();

            if (!_kits.Instance.ContainsKey(key))
                return 0;

            var kit = KitsPlusPlugin.Instance.KitDb.GetKitByName(player, kitName);

            if (kit == null)
                return 0;

            var start = _kits.Instance[key];

            var waitTime = (DateTime.Now - start).TotalSeconds;

            int timeLeft = (kit.Cooldown - Convert.ToInt32(waitTime));

            if(timeLeft <= 0)
            {
                timeLeft = 0;

                _kits.Instance.Remove(key);
            }

            return timeLeft;
        }

        public void SetKitCooldown(IRocketPlayer player, string kitName)
        {
            var kit = KitsPlusPlugin.Instance.KitDb.GetKitByName(player, kitName);

            if (kit == null)
                return;

            var key = $"{player.Id}_{kitName}".ToLower();

            if (_kits.Instance.ContainsKey(key))
            {
                _kits.Instance.Remove(key);
            }

            _kits.Instance.Add(key, DateTime.Now);
        }

        public bool CheckCooldown(IRocketPlayer player, Kit kit)
        {

            var globalTimeLeft = GetTimeLeftForGlobal(player.Id);

            if (globalTimeLeft > 0)
            {
                UnturnedChat.Say(player, KitsPlusPlugin.Instance.Translate("command_kit_global_cooldown", globalTimeLeft), true);
                return true;
            }

            var kitTimeLeft = GetTimeLeftForKit(player, kit.Name);

            if (kitTimeLeft > 0)
            {
                UnturnedChat.Say(player, KitsPlusPlugin.Instance.Translate("command_kit_cooldown", kitTimeLeft, kit.Name), true);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            Logger.Log("Saving global cooldowns...");
            _global.Save();
            _global = null;

            Logger.Log("Saving kit cooldowns...");
            _kits.Save();
            _kits = null;
        }
    }
}
