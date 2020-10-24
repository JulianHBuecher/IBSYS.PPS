using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "batch")]
	public class Batch
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
		[XmlAttribute(AttributeName = "cycletime")]
		public string Cycletime { get; set; }
		[XmlAttribute(AttributeName = "cost")]
		public string Cost { get; set; }
	}
}
