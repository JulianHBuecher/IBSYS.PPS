using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "defectivegoods")]
	public class Defectivegoods
	{
		[XmlElement(ElementName = "quantity")]
		public Quantity Quantity { get; set; }
		[XmlElement(ElementName = "costs")]
		public Costs Costs { get; set; }
	}
}
