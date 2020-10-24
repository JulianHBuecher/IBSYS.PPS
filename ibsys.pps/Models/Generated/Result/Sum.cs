using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "sum")]
	public class Sum
	{
		[XmlAttribute(AttributeName = "setupevents")]
		public string Setupevents { get; set; }
		[XmlAttribute(AttributeName = "idletime")]
		public string Idletime { get; set; }
		[XmlAttribute(AttributeName = "wageidletimecosts")]
		public string Wageidletimecosts { get; set; }
		[XmlAttribute(AttributeName = "wagecosts")]
		public string Wagecosts { get; set; }
		[XmlAttribute(AttributeName = "machineidletimecosts")]
		public string Machineidletimecosts { get; set; }
	}
}
