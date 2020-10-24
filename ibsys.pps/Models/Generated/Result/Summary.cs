using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "summary")]
	public class Summary
	{
		[XmlElement(ElementName = "profit")]
		public Profit Profit { get; set; }
	}
}
