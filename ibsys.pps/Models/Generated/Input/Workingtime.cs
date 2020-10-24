using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Workingtime
    {
        [XmlAttribute(AttributeName = "station")]
        public string station { get; set; }
        [XmlAttribute(AttributeName = "shift")]
        public string shift { get; set; }
        [XmlAttribute(AttributeName = "overtime")]
        public string overtime { get; set; }
    }
}