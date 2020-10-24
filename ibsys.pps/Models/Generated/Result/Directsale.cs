using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "directsale")]
	public class Directsale
	{
		[XmlElement(ElementName = "profit")]
		public Profit Profit { get; set; }
		[XmlElement(ElementName = "contractpenalty")]
		public Contractpenalty Contractpenalty { get; set; }
	}
}
