using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "missingpart")]
	public class Missingpart
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}
}
