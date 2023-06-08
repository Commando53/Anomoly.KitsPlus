using Anomoly.KitsPlus.Managers;
using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Commands
{
    public class CommandDeleteKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "deletekit";

        public string Help => "Delete a kit";

        public string Syntax => "<kit name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "deletekit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Length == 0 || command.Length > 1)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_deletekit_invalid", Syntax), true);
                return;
            }

            var kitName = command[0];

            var deleted = KitsPlusPlugin.Instance.KitManager.DeleteKit(kitName);

            UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_deletekit_deleted", kitName, deleted), true);
        }
    }
}
