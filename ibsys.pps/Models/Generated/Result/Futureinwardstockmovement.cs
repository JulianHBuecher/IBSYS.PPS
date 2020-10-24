using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "futureinwardstockmovement")]
	public class Futureinwardstockmovement
	{
		[XmlElement(ElementName = "order")]
		public List<Order> Order { get; set; }
	}
}
