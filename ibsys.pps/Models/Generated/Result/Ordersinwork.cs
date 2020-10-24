using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "ordersinwork")]
	public class Ordersinwork
	{
		[XmlElement(ElementName = "workplace")]
		public List<Workplace> Workplace { get; set; }
	}
}
