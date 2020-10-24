using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Production
    {
        [XmlAttribute(AttributeName = "article")]
        public string article { get; set; }
        [XmlAttribute(AttributeName = "quantity")]
        public int quantity { get; set; }
    }
}