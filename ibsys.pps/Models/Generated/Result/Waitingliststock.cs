using System.Collections.Generic;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{
    [XmlRoot(ElementName = "waitingliststock")]
	public class Waitingliststock
	{
		[XmlElement(ElementName = "missingpart")]
		public List<Missingpart> Missingpart { get; set; }
	}
}
