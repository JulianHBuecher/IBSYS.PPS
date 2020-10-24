using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class User
    {
        [XmlAttribute(AttributeName = "game")]
        public string Game { get; set; }
        [XmlAttribute(AttributeName = "group")]
        public string Group { get; set; }
        [XmlAttribute(AttributeName = "period")]
        public int Period { get; set; }
    }
}