using Anomoly.KitsPlus.Data;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Anomoly.KitsPlus
{
    public class KitsPlusConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseType { get; set; }
        public string MySQLConnectionString { get; set; }
        public string MySQLTablePrefix { get; set; }

        public int GlobalCooldown { get; set; }
        public void LoadDefaults()
        {
            DatabaseType = "json";
            GlobalCooldown = 10;
            MySQLConnectionString = "Server=localhost;Port=3306;Database=unturned;Uid=root;Pwd=pwd;";
            MySQLTablePrefix = "myserver";
        }
    }
}
