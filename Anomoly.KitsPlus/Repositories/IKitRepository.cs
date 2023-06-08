using Anomoly.KitsPlus.Data;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Databases
{
    public interface IKitRepository: IDisposable
    {
        string Name { get; }

        void Initialize(Kit[] defaultKits);

        List<Kit> GetKits();

        Kit GetKitByName(string name);

        bool CreateKit(Kit kit);
        int DeleteKit(string name);

        void Reset();
    }
}
