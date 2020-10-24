using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "result")]
	public class Result
	{
		[XmlElement(ElementName = "general")]
		public General General { get; set; }
		[XmlElement(ElementName = "defectivegoods")]
		public Defectivegoods Defectivegoods { get; set; }
		[XmlElement(ElementName = "normalsale")]
		public Normalsale Normalsale { get; set; }
		[XmlElement(ElementName = "directsale")]
		public Directsale Directsale { get; set; }
		[XmlElement(ElementName = "marketplacesale")]
		public Marketplacesale Marketplacesale { get; set; }
		[XmlElement(ElementName = "summary")]
		public Summary Summary { get; set; }
	}
}
