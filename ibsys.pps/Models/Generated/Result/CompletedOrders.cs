using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
	[XmlRoot(ElementName = "completedorders")]
	public class Completedorders
	{
		[XmlElement(ElementName = "order")]
		public List<Order> Order { get; set; }
	}
}
