using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "inwardstockmovement")]
	public class Inwardstockmovement
	{
		[XmlElement(ElementName = "order")]
		public List<Order> Order { get; set; }
	}
}
