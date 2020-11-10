using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Order
    {
        [XmlAttribute("article")]
        public string Article { get; set; }
        [XmlAttribute("quantity")]
        public string Quantity { get; set; }
        [XmlAttribute("modus")]
        public string Modus { get; set; }
    }
}