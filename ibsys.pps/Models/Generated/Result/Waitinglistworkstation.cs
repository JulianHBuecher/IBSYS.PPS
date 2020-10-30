using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "waitinglistworkstations")]
	public class Waitinglistworkstations
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[XmlIgnore]
		[JsonIgnore]
		public int ID { get; set; }
		[XmlElement(ElementName = "workplace")]
		public List<Workplace> Workplace { get; set; }
	}
}
