using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "waitinglistworkstations")]
	public class Waitinglistworkstations
	{
		[XmlElement(ElementName = "workplace")]
		public List<Workplace> Workplace { get; set; }
	}
}
