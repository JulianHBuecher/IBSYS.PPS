using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "workplace")]
	public class Workplace
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
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
		[XmlAttribute(AttributeName = "timeneed")]
		public string Timeneed { get; set; }
		[XmlElement(ElementName = "waitinglist")]
		public List<Waitinglist> Waitinglist { get; set; }
		[XmlAttribute(AttributeName = "period")]
		public string Period { get; set; }
		[XmlAttribute(AttributeName = "order")]
		public string Order { get; set; }
		[XmlAttribute(AttributeName = "batch")]
		public string Batch { get; set; }
		[XmlAttribute(AttributeName = "item")]
		public string Item { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
	}
}
