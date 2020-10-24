using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "results")]
		public class Results
		{
			[XmlElement(ElementName = "forecast")]
			public Forecast Forecast { get; set; }
			[XmlElement(ElementName = "warehousestock")]
			public Warehousestock Warehousestock { get; set; }
			[XmlElement(ElementName = "inwardstockmovement")]
			public Inwardstockmovement Inwardstockmovement { get; set; }
			[XmlElement(ElementName = "futureinwardstockmovement")]
			public Futureinwardstockmovement Futureinwardstockmovement { get; set; }
			[XmlElement(ElementName = "idletimecosts")]
			public Idletimecosts Idletimecosts { get; set; }
			[XmlElement(ElementName = "waitinglistworkstations")]
			public Waitinglistworkstations Waitinglistworkstations { get; set; }
			[XmlElement(ElementName = "waitingliststock")]
			public Waitingliststock Waitingliststock { get; set; }
			[XmlElement(ElementName = "ordersinwork")]
			public Ordersinwork Ordersinwork { get; set; }
			[XmlElement(ElementName = "completedorders")]
			public Completedorders Completedorders { get; set; }
			[XmlElement(ElementName = "cycletimes")]
			public Cycletimes Cycletimes { get; set; }
			[XmlElement(ElementName = "result")]
			public Result Result { get; set; }
			[XmlAttribute(AttributeName = "game")]
			public string Game { get; set; }
			[XmlAttribute(AttributeName = "group")]
			public string Group { get; set; }
			[XmlAttribute(AttributeName = "period")]
			public string Period { get; set; }
	}
}
