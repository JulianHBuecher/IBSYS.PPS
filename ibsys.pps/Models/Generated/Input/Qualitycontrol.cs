using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Qualitycontrol
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "losequantity")]
        public string LoseQuantity { get; set; }
        [XmlAttribute(AttributeName = "delay")]
        public string Delay { get; set; }
    }
}