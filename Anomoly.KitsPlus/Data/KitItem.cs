using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Anomoly.KitsPlus.Data
{
    [Serializable]
    public class KitItem
    {
        [XmlAttribute("id")]
        public ushort Id { get; set; }

        [XmlAttribute("amount")]
        public byte Amount { get; set; }
    }
}
