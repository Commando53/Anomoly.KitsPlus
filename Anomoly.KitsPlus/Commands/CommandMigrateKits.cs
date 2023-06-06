using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using System.Collections.Generic;

namespace Anomoly.KitsPlus.Commands
{
    public class CommandMigrateKits : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "migratekits";

        public string Help => "Migrate kits from the Old RocketMod kits plugin";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {

            var isConsolePlayer = caller is ConsolePlayer;

            if (!RocketPlugin.IsDependencyLoaded("Kits"))
            {
                var msg = KitsPlusPlugin.Instance.Translate("command_migratekits_no_plugin");
                if (isConsolePlayer)
                {
                    Logger.LogWarning(msg);
                }
                else
                {
                    UnturnedChat.Say(caller, msg, true);
                }

                return;
            }

            var config = KitsPlusPlugin.Instance.Configuration.Instance;

            var newKits = new List<Data.Kit>();

            RocketPlugin.ExecuteDependencyCode("Kits", (IRocketPlugin plugin) =>
            {
                var kitsPlugin = (fr34kyn01535.Kits.Kits)plugin;

                kitsPlugin.Configuration.Instance.Kits.ForEach(k =>
                {

                    var kitCooldown = k.Cooldown.Value;

                    newKits.Add(new Data.Kit()
                    {
                        Name = k.Name,
                        XP = k.XP,
                        Vehicle = k.Vehicle,
                        MaxUsage = 0,
                        Cooldown = kitCooldown,
                        Items = k.Items.ConvertAll(i =>
                        {
                            var item = new Data.KitItem()
                            {
                                Id = i.ItemId,
                                Amount = i.Amount,
                            };

                            return item;
                        }),
                        
                    });
                });
            });

            int migrated = 0;
            int failed = 0;
            newKits.ForEach(k =>
            {
                var created = KitsPlusPlugin.Instance.KitDb.CreateKit(k);
                if (created)
                    migrated++;
                else
                    failed++;
            });


            var msgs = new string[]
            {
                KitsPlusPlugin.Instance.Translate("command_migratekits_migrated", migrated, failed),
                KitsPlusPlugin.Instance.Translate("command_migratekits_warning")
            };

            if (isConsolePlayer)
            {
                for(int i = 0; i < msgs.Length; i++)
                {
                    Logger.Log(msgs[i]);
                }
            }
            else
            {
                for (int i = 0; i < msgs.Length; i++)
                {
                    UnturnedChat.Say(caller, msgs[i], true);
                }
            }
        }
    }
}
