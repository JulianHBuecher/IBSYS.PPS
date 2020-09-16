using System.Xml.Serialization;

namespace IBSYS.PPS.Models
{
    public class Qualitycontrol
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "losequantity")]
        public int LoseQuantity { get; set; }
        [XmlAttribute(AttributeName = "delay")]
        public int Delay { get; set; }
    }
}