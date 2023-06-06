using Anomoly.KitsPlus.Data;
using Anomoly.KitsPlus.Databases;
using Anomoly.KitsPlus.Managers;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus
{
    public class KitsPlusPlugin: RocketPlugin<KitsPlusConfiguration>
    {
        public static KitsPlusPlugin Instance { get; private set; }

        public CooldownManager CooldownManager { get; private set; }
        public UsageManager UsageManager { get; private set; }
        public IKitDatabase KitDb { get; set; }

        protected override void Load()
        {
            base.Load();

            Instance = this;
            CooldownManager = new CooldownManager();
            UsageManager = new UsageManager();

            switch (Configuration.Instance.DatabaseType.ToLower())
            {
                case "json":
                    KitDb = new JsonDatabase();
                    break;
                case "mysql":
                    KitDb = new MySQLDatabase();
                    break;
                default:
                    Logger.Log("Defaulted to JSON DB");
                    KitDb = new JsonDatabase();
                    break;
            }


            Logger.Log($"{string.Format("KitsPlus v{0}", Assembly.GetName().Version.ToString())} by Anomoly has loaded!");
            Logger.Log("Need support? Join my Discord @ https://discord.gg/rVH9e7Kj9y");
            Logger.LogWarning("When reloading, cooldowns will refresh");
        }

        protected override void Unload()
        {
            base.Unload();

            Instance = null;
            CooldownManager = null;
            KitDb.Dispose();
            KitDb = null;

            Logger.Log("KitsPlus by Anomoly has unloaded!");
            Logger.Log("Need support? Join my Discord @ https://discord.gg/rVH9e7Kj9y");
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"command_kit_invalid", "Please do /kit {0}!" },
            {"command_kit_not_found", "Kit not found! View available kits using /kits" },
            {"command_kit_global_cooldown", "Please wait {0} seconds before redeeming another kit." },
            {"command_kit_cooldown","Please wait {0} seconds before redeeming the '{1}' kit again." },
            {"command_kit_max_usage","You have used {0}/{0} uses of the '{1}' kit!" },
            {"command_kit_usage_left", "You have {0} uses left!" },
            {"command_kit_redeemed","You have successfully redeemed the '{0}' kit!" },
            {"command_kits_list","Available kits: {0}" },
            {"command_createkit_invalid","Please do /createkit {0}!" },
            {"command_createkit_created","Successfully created the '{0}' kit!" },
            {"command_createkit_existing_name","A kit already exists with the name of '{0}'!" },
            {"command_createkit_failed","Failed to create '{0}' kit." },
            {"command_giftkit_invalid","Please do /givekit {0}!" },
            {"command_giftkit_no_player","No player could be found by the name of '{0}'." },
            {"command_giftkit_no_kit", "No kit could be found by the name of '{0}'." },
            {"command_giftkit_success","Successfully given your '{0}' kit to {1}" },
            {"command_giftkit_failed","Failed to give {0} the '{1}' kit!" },
            {"command_giftkit_gifted","{0} has gifted you their '{1}' kit!" },
            {"command_giftkit_self","You cannot gift yourself a kit!" },
            {"command_deletekit_invalid","Please do /deletekit {0}!" },
            {"command_deletekit_deleted","Deleted {0} kit(s) with the name of '{1}'." }
        };
    }
}
