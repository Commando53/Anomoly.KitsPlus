using Anomoly.KitsPlus.Data;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Databases
{
    public interface IKitDatabase: IDisposable
    {
        string Name { get; }
        
        List<Kit> GetKits();
        List<Kit> GetKits(IRocketPlayer player);

        Kit GetKitByName(string name);
        Kit GetKitByName(IRocketPlayer player, string name);

        bool CreateKit(Kit kit);
        int DeleteKit(string name);
    }
}
