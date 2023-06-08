using Anomoly.KitsPlus.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Databases.Migrations
{
    public interface IMySQLMigration
    {
        string Name { get; }
        void Up(DbConnection connection);
        void Down(DbConnection connection);
    }
}
