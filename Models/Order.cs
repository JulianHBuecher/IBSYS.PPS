using System.Xml.Serialization;

namespace IBSYS.PPS.Models
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