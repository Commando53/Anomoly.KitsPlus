using Rocket.API;

namespace Anomoly.KitsPlus
{
    public class KitsPlusConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseType { get; set; }
        public string MySQLConnectionString { get; set; }
        public string MySQLTablePrefix { get; set; }

        public bool KitUsagesEnabled { get; set; }
        public bool DisplayKitUsagesOnList { get; set; }
        public string PreferredCurrencyName { get; set; }
        public int GlobalCooldown { get; set; }
        public void LoadDefaults()
        {
            PreferredCurrencyName = "$";
            DatabaseType = "json";
            MySQLConnectionString = "Server=localhost;Port=3306;Database=unturned;Uid=root;Pwd=pwd;";
            MySQLTablePrefix = "myserver";
            KitUsagesEnabled = true;
            DisplayKitUsagesOnList = true;
            GlobalCooldown = 10;
        }
    }
}
