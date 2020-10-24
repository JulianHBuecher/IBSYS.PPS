using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "normalsale")]
	public class Normalsale
	{
		[XmlElement(ElementName = "salesprice")]
		public Salesprice Salesprice { get; set; }
		[XmlElement(ElementName = "profit")]
		public Profit Profit { get; set; }
		[XmlElement(ElementName = "profitperunit")]
		public Profitperunit Profitperunit { get; set; }
	}
}
