using Anomoly.Core.Library.UnturnedStore;
using Anomoly.KitsPlus.Databases;
using Anomoly.KitsPlus.Managers;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Anomoly.KitsPlus
{
    public class KitsPlusPlugin: RocketPlugin<KitsPlusConfiguration>
    {
        public static KitsPlusPlugin Instance { get; private set; }

        public KitManager KitManager { get; private set; }
        public CooldownManager CooldownManager { get; private set; }
        public UsageManager UsageManager { get; private set; }

        public const int US_PLUGIN_ID = 1470;

        protected override void Load()
        {
            base.Load();

            Instance = this;
            KitManager = new KitManager();
            CooldownManager = new CooldownManager();
            if(Configuration.Instance.KitUsagesEnabled)
                UsageManager = new UsageManager();


            Logger.Log($"{string.Format("KitsPlus v{0}", Assembly.GetName().Version.ToString())} by Anomoly has loaded!");
            Logger.Log("Need support? Join my Discord @ https://discord.gg/rVH9e7Kj9y");

            bool isUpdateToDate = UnturnedStoreAPI.IsUpdateToDate(US_PLUGIN_ID, Assembly.GetName().Version);
            if (!isUpdateToDate)
                Logger.LogWarning("[Update Detected] KitsPlus has an update! Please download the latest version @ https://unturnedstore.com/products/1470");
        }

        protected override void Unload()
        {
            base.Unload();

            Instance = null;

            CooldownManager.Dispose();
            CooldownManager = null;

            UsageManager.Dispose();
            UsageManager = null;

            KitManager.Dispose();
            KitManager = null;

            Logger.Log("KitsPlus by Anomoly has unloaded!");
            Logger.Log("Need support? Join my Discord @ https://discord.gg/rVH9e7Kj9y");
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"command_kit_invalid", "Please do /kit {0}!" },
            {"command_kit_not_found", "Kit not found! View available kits using /kits" },
            {"command_kit_global_cooldown", "Please wait {0} before redeeming another kit." },
            {"command_kit_cooldown","Please wait {0} before redeeming the '{1}' kit again." },
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
            {"command_giftkit_gifted","{0} has gifted you their '{1}' kit!" },
            {"command_giftkit_self","You cannot gift yourself a kit!" },
            {"command_deletekit_invalid","Please do /deletekit {0}!" },
            {"command_deletekit_deleted","Kit '{0}' deleted: {1}." },
            {"command_migratekits_migrated","Migrated {0} kits succesfully with {1} failures. " },
            {"command_migratekits_warning","Please shutdown the server and remove the old Kits plugin. Restart only after removing!" },
            {"command_migratekits_no_plugin","Failed to find Kits plugin. Please make sure to restart the server with the plugin installed." },
            {"command_resetkits_invalid","Please do /resetkits {0}!" },
            {"command_resetkits_all", "Reset kits data, cooldowns, and usages!" },
            {"command_resetkits_cooldowns", "Reset kit & global cooldowns!" },
            {"command_resetkits_usages", "Reset kit usages!" },
            {"command_resetkits_usages_disabled", "Failed to reset kit usages as Kit Usages are disabled!" }
        };
    }
}
