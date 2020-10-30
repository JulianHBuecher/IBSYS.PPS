using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "waitinglist")]
	public class Waitinglist
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[XmlIgnore]
		[JsonIgnore]
		public string ID { get; set; }
		[XmlAttribute(AttributeName = "period")]
		public string Period { get; set; }
		[XmlAttribute(AttributeName = "order")]
		public string Order { get; set; }
		[XmlAttribute(AttributeName = "firstbatch")]
		public string Firstbatch { get; set; }
		[XmlAttribute(AttributeName = "lastbatch")]
		public string Lastbatch { get; set; }
		[XmlAttribute(AttributeName = "item")]
		public string Item { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
		[XmlAttribute(AttributeName = "timeneed")]
		public string Timeneed { get; set; }
	}
}
