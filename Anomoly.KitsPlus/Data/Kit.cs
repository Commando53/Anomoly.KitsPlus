using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Anomoly.KitsPlus.Data
{
    [Serializable]
    public class Kit
    {
        public string Name { get; set; }

        public uint? XP { get; set; }

        public ushort? Vehicle { get; set; }

        public int Cooldown { get; set; }

        public int MaxUsage { get; set; } = 0;

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<KitItem> Items { get; set; }
    }
}
