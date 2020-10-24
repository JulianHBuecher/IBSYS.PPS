using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "article")]
	public class Article
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
		[XmlAttribute(AttributeName = "startamount")]
		public string Startamount { get; set; }
		[XmlAttribute(AttributeName = "pct")]
		public string Pct { get; set; }
		[XmlAttribute(AttributeName = "price")]
		public string Price { get; set; }
		[XmlAttribute(AttributeName = "stockvalue")]
		public string Stockvalue { get; set; }
	}
}
