using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Order
    {
        [XmlAttribute]
        public string article { get; set; }
        [XmlAttribute]
        public int quantity { get; set; }
        [XmlAttribute]
        public string modus { get; set; }
    }
}