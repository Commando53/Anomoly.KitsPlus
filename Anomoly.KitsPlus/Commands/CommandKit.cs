using Anomoly.KitsPlus.Managers;
using Anomoly.KitsPlus.Utils;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Commands
{
    public class CommandKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kit";

        public string Help => "Redeem a kit";

        public string Syntax => "<kit name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "kit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Length == 0 || command.Length > 1)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kit_invalid", Syntax), true);
                return;
            }

            var kitName = command[0];

            var kit = KitsPlusPlugin.Instance.KitManager.GetPlayerKit(caller, kitName);

            if(kit == null)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kit_not_found"), true);
                return;
            }

            var usageEnabled = KitsPlusPlugin.Instance.Configuration.Instance.KitUsagesEnabled && kit.MaxUsage > 0;

            if(usageEnabled && KitsPlusPlugin.Instance.UsageManager.GetKitUsage(caller.Id, kit.Name) >= kit.MaxUsage)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kit_max_usage", kit.MaxUsage, kit.Name), true);
                return;
            }

            bool isOnCooldown = KitsPlusPlugin.Instance.CooldownManager.CheckCooldown(caller, kit);
            if (isOnCooldown)
                return;

            KitsPlusPlugin.Instance.KitManager.GiveKit(caller, kit);
            UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kit_redeemed", kit.Name), true);
        }
    }
}
