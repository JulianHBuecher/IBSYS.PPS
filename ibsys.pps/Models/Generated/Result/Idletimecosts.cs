using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "idletimecosts")]
	public class Idletimecosts
	{
		[XmlElement(ElementName = "workplace")]
		public List<Workplace> Workplace { get; set; }
		[XmlElement(ElementName = "sum")]
		public Sum Sum { get; set; }
		[XmlAttribute(AttributeName = "current")]
		public string Current { get; set; }
		[XmlAttribute(AttributeName = "average")]
		public string Average { get; set; }
		[XmlAttribute(AttributeName = "all")]
		public string All { get; set; }
	}
}
