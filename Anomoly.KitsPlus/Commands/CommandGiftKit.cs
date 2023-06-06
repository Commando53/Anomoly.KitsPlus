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
    public class CommandGiftKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "giftkit";

        public string Help => "Gift another player your kit";

        public string Syntax => "<player> <kit name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>()
        {
            "giftkit"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {

            if(command.Length == 0 || command.Length > 2)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_giftkit_invalid", Syntax), true);
                return;
            }

            var targetName = command[0];

            UnturnedPlayer targetPlayer = UnturnedPlayer.FromName(targetName);

            if(targetPlayer == null)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_giftkit_no_player", targetName), true);
                return;
            }

            if(targetPlayer.Id == caller.Id)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_giftkit_self"), true);
                return;
            }

            var kitName = command[1];

            var kit = KitsPlusPlugin.Instance.KitDb.GetKitByName(caller, kitName);

            if(kit == null)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_giftkit_no_kit", kitName), true);
                return;
            }

            var isConsolePlayer = caller is ConsolePlayer;

            var usageEnabled = KitsPlusPlugin.Instance.Configuration.Instance.KitUsagesEnabled && kit.MaxUsage > 0;

            if(!isConsolePlayer && usageEnabled && KitsPlusPlugin.Instance.UsageManager.GetKitUsage(caller.Id, kit.Name) >= kit.MaxUsage)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kit_max_usage", kit.MaxUsage, kit.Name), true);
                return;
            }


            var cMgr = KitsPlusPlugin.Instance.CooldownManager;

            if (!isConsolePlayer && cMgr.CheckCooldown(caller, kit))
                return;


            bool success = targetPlayer.GiveKit(kit);
            
            if (!success)
            {

                var failedMsg = KitsPlusPlugin.Instance.Translate("command_giftkit_failed", targetPlayer.DisplayName, kit.Name);

                if (isConsolePlayer)
                {
                    Logger.LogError(failedMsg);
                }
                else
                {
                    UnturnedChat.Say(caller, failedMsg, true);
                }
               
                return;
            }

            if(!isConsolePlayer)
            {
                cMgr.SetKitCooldown(caller, kit.Name);
                cMgr.SetGlobalCooldown(caller.Id);


                if (usageEnabled)
                    KitsPlusPlugin.Instance.UsageManager.AddUsage(caller.Id, kit.Name);
            }

            var msg = KitsPlusPlugin.Instance.Translate("command_giftkit_success", kit.Name, targetPlayer.DisplayName);

            if (isConsolePlayer)
            {
                Logger.Log(msg);
            }
            else
            {
                UnturnedChat.Say(caller, msg, true);
                if (usageEnabled)
                {
                    var uses = KitsPlusPlugin.Instance.UsageManager.GetKitUsage(caller.Id, kit.Name);
                    UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kit_usage_left", (kit.MaxUsage - uses)), true);
                }
            }

            
            UnturnedChat.Say(targetPlayer, KitsPlusPlugin.Instance.Translate("command_giftkit_gifted", caller.DisplayName, kit.Name), true);
        }
    }
}
