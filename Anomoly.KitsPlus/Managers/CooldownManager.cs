using Anomoly.KitsPlus.Data;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Managers
{
    public class CooldownManager
    {
        private Dictionary<string, DateTime> _global;
        private Dictionary<string, DateTime> _kits;

        public int GlobalCooldown => KitsPlusPlugin.Instance.Configuration.Instance.GlobalCooldown;

        public CooldownManager()
        {
            Logger.Log("Initializing CooldownManager...");
            _global = new Dictionary<string, DateTime>();
            _kits = new Dictionary<string, DateTime>();
        }


        public int GetTimeLeftForGlobal(string playerId)
        {
            if (_global.ContainsKey(playerId))
            {
                var start = _global[playerId];

                var waitTime = (DateTime.Now - start).TotalSeconds;

                int timeLeft = (GlobalCooldown - Convert.ToInt32(waitTime));

                if (timeLeft <= 0)
                {
                    timeLeft = 0;
                    _global.Remove(playerId);
                }

                return timeLeft;
            }

            return 0;
        }

        public void SetGlobalCooldown(string playerId)
        {
            if (GlobalCooldown <= 0)
                return;

            _global.Add(playerId, DateTime.Now);
        }

        public int GetTimeLeftForKit(IRocketPlayer player, string kitName)
        {
            var key = $"{player.Id}_{kitName}".ToLower();

            if (!_kits.ContainsKey(key))
                return 0;

            var kit = KitsPlusPlugin.Instance.KitDb.GetKitByName(player, kitName);

            if (kit == null)
                return 0;

            var start = _kits[key];

            var waitTime = (DateTime.Now - start).TotalSeconds;

            int timeLeft = (kit.Cooldown - Convert.ToInt32(waitTime));

            if(timeLeft <= 0)
            {
                timeLeft = 0;

                _kits.Remove(key);
            }

            return timeLeft;
        }

        public void SetKitCooldown(IRocketPlayer player, string kitName)
        {
            var kit = KitsPlusPlugin.Instance.KitDb.GetKitByName(player, kitName);

            if (kit == null)
                return;

            var key = $"{player.Id}_{kitName}".ToLower();

            if (_kits.ContainsKey(key))
            {
                _kits.Remove(key);
            }

            _kits.Add(key, DateTime.Now);
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
    }
}
