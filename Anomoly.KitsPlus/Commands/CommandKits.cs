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
    public class CommandKits : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kits";

        public string Help => "View a list of available kits";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "kits" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var kits = KitsPlusPlugin.Instance.KitDb.GetKits(caller);

            var names = kits.Select(x => x.Name).ToArray();

            var msg = names.Length > 0 ? string.Join(",", names) : "No kits available";


            UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_kits_list", msg), true);
        }
    }
}
