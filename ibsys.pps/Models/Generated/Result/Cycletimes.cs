using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "cycletimes")]
	public class Cycletimes
	{
		[XmlAttribute(AttributeName = "startedorders")]
		public string Startedorders { get; set; }
		[XmlAttribute(AttributeName = "waitingorders")]
		public string Waitingorders { get; set; }
	}
}
