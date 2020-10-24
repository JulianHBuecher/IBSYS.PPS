using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "order")]
	public class Order
	{
		[Key]
		[JsonIgnore]
		[XmlIgnore]
		public int? OrderID { get; set; }
		[XmlAttribute(AttributeName = "orderperiod")]
		public string Orderperiod { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "mode")]
		public string Mode { get; set; }
		[XmlAttribute(AttributeName = "article")]
		public string Article { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
		[XmlAttribute(AttributeName = "time")]
		public string Time { get; set; }
		[XmlAttribute(AttributeName = "materialcosts")]
		public string Materialcosts { get; set; }
		[XmlAttribute(AttributeName = "ordercosts")]
		public string Ordercosts { get; set; }
		[XmlAttribute(AttributeName = "entirecosts")]
		public string Entirecosts { get; set; }
		[XmlAttribute(AttributeName = "piececosts")]
		public string Piececosts { get; set; }
		[XmlElement(ElementName = "batch")]
		public List<Batch> Batch { get; set; }
		[XmlAttribute(AttributeName = "period")]
		public string Period { get; set; }
		[XmlAttribute(AttributeName = "item")]
		public string Item { get; set; }
		[XmlAttribute(AttributeName = "quantity")]
		public string Quantity { get; set; }
		[XmlAttribute(AttributeName = "cost")]
		public string Cost { get; set; }
		[XmlAttribute(AttributeName = "averageunitcosts")]
		public string Averageunitcosts { get; set; }
	}
}
