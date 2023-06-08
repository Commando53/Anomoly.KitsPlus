using Anomoly.KitsPlus.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Databases.Migrations
{
    public class MaxUsage_Migration : IMySQLMigration
    {
        public string Name => "maxusage_migration";

        public void Down(DbConnection connection)
        {
            var table = MySQLRepository.KitsTable;
            connection.ExecuteUpdate($"ALTER TABLE `{table}` DROP COLUMN `maxusage`;");
        }

        public void Up(DbConnection connection)
        {
            var table = MySQLRepository.KitsTable;

            connection.ExecuteUpdate($"ALTER TABLE `{table}` ADD `maxusage` int NOT NULL DEFAULT(0);");
        }
    }
}
