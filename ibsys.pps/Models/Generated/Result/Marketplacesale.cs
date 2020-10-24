using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "marketplacesale")]
	public class Marketplacesale
	{
		[XmlElement(ElementName = "profit")]
		public Profit Profit { get; set; }
	}
}
