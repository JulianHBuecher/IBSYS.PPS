using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "contractpenalty")]
	public class Contractpenalty
	{
		[XmlAttribute(AttributeName = "current")]
		public string Current { get; set; }
		[XmlAttribute(AttributeName = "average")]
		public string Average { get; set; }
		[XmlAttribute(AttributeName = "all")]
		public string All { get; set; }
	}
}
