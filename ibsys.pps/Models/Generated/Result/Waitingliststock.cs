using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "waitingliststock")]
	public class Waitingliststock
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[XmlIgnore]
		[JsonIgnore]
		public int ID { get; set; }
		[XmlElement(ElementName = "missingpart")]
		public List<Missingpart> Missingpart { get; set; }
	}
}
