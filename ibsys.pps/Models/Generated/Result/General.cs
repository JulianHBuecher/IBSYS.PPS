using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
	[XmlRoot(ElementName = "general")]
	public class General
	{
		[XmlElement(ElementName = "capacity")]
		public Capacity Capacity { get; set; }
		[XmlElement(ElementName = "possiblecapacity")]
		public Possiblecapacity Possiblecapacity { get; set; }
		[XmlElement(ElementName = "relpossiblenormalcapacity")]
		public Relpossiblenormalcapacity Relpossiblenormalcapacity { get; set; }
		[XmlElement(ElementName = "productivetime")]
		public Productivetime Productivetime { get; set; }
		[XmlElement(ElementName = "effiency")]
		public Effiency Effiency { get; set; }
		[XmlElement(ElementName = "sellwish")]
		public Sellwish Sellwish { get; set; }
		[XmlElement(ElementName = "salesquantity")]
		public Salesquantity Salesquantity { get; set; }
		[XmlElement(ElementName = "deliveryreliability")]
		public Deliveryreliability Deliveryreliability { get; set; }
		[XmlElement(ElementName = "idletime")]
		public Idletime Idletime { get; set; }
		[XmlElement(ElementName = "idletimecosts")]
		public Idletimecosts Idletimecosts { get; set; }
		[XmlElement(ElementName = "storevalue")]
		public Storevalue Storevalue { get; set; }
		[XmlElement(ElementName = "storagecosts")]
		public Storagecosts Storagecosts { get; set; }
	}
}
