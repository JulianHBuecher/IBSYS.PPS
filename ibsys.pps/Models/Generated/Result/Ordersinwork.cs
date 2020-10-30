using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "ordersinwork")]
	public class Ordersinwork
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[XmlIgnore]
		[JsonIgnore]
		public int OrdersInWorkID { get; set; }
		[XmlElement(ElementName = "workplace")]
		public List<Workplace> Workplace { get; set; }
	}
}
