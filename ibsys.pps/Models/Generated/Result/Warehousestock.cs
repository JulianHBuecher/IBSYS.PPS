using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "warehousestock")]
	public class Warehousestock
	{
		[XmlElement(ElementName = "article")]
		public List<Article> Article { get; set; }
		[XmlElement(ElementName = "totalstockvalue")]
		public string Totalstockvalue { get; set; }
	}
}
