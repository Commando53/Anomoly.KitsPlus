using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Commands
{
    public class CommandResetKits : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "resetkits";

        public string Help => "Reset usages, cooldowns, and kits data.";

        public string Syntax => "[all|usages|cooldowns|data]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "resetkits" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Length == 0 || command.Length > 1)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_invalid", Syntax), true);
                return;
            }

            switch (command[0].ToLower())
            {
                case "all":
                    KitsPlusPlugin.Instance.KitManager.Reset();
                    KitsPlusPlugin.Instance.CooldownManager.Reset();
                    if(KitsPlusPlugin.Instance.Configuration.Instance.KitUsagesEnabled)
                        KitsPlusPlugin.Instance.UsageManager.Reset();
                    UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_all"), true);
                    break;
                case "cooldowns":
                    KitsPlusPlugin.Instance.CooldownManager.Reset();
                    UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_cooldowns"), true);
                    break;
                case "data":
                    KitsPlusPlugin.Instance.KitManager.Reset();
                    UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_data"), true);
                    break;
                case "usages":
                    if(KitsPlusPlugin.Instance.Configuration.Instance.KitUsagesEnabled)
                    {
                        KitsPlusPlugin.Instance.UsageManager.Reset();
                        UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_usages"), true);
                    }
                    else
                    {
                        UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_usages_disabled"), true);
                    }
                    break;
                default:
                    UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_resetkits_invalid", Syntax), true);
                    return;
            }
        }
    }
}
