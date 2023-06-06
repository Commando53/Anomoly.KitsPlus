using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Managers;
using Anomoly.KitsPlus.Utils;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Commands
{
    public class CommandCreateKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "createkit";

        public string Help => "Create a new kit from items in your inventory";

        public string Syntax => "<name> [<cooldown>] [<vehicle>] [<xp>]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "createkit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Length == 0 || command.Length > 4)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_invalid", Syntax), true);
                return;
            }

            var name = command[0];
            int cooldown = 0;
            ushort vehicle = 0;
            uint xp = 0;

            if(command.Length >= 2 && !int.TryParse(command[1], out cooldown))
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_invalid", Syntax), true);
                return;
            }

            if (command.Length >= 3 && !ushort.TryParse(command[2], out vehicle))
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_invalid", Syntax), true);
                return;
            }

            if(command.Length == 4 && !uint.TryParse(command[3], out xp))
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_invalid", Syntax), true);
                return;
            }

            var player = (UnturnedPlayer)caller;

            var kit = new Kit()
            {
                Name = name,
                XP = xp,
                Vehicle = vehicle,
                Cooldown = cooldown,
                Items = player.GetKitItemsFromInventory(),
            };

            var existingKit = KitsPlusPlugin.Instance.KitDb.GetKitByName(kit.Name);

            if(existingKit != null)
            {
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_existing_name", kit.Name), true);
                return;
            }


            bool createdKit = KitsPlusPlugin.Instance.KitDb.CreateKit(kit);
            if (createdKit)
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_created", kit.Name), true);
            else
                UnturnedChat.Say(caller, KitsPlusPlugin.Instance.Translate("command_createkit_failed", kit.Name), true);
        }
    }
}
