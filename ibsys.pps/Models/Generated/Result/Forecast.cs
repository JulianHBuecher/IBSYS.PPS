using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "forecast")]
	public class Forecast
	{
		[XmlAttribute(AttributeName = "p1")]
		public string P1 { get; set; }
		[XmlAttribute(AttributeName = "p2")]
		public string P2 { get; set; }
		[XmlAttribute(AttributeName = "p3")]
		public string P3 { get; set; }
	}
}
