using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Production
    {
        [XmlAttribute(AttributeName = "article")]
        public string Article { get; set; }
        [XmlAttribute(AttributeName = "quantity")]
        public string Quantity { get; set; }
    }
}