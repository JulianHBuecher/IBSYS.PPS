using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class Workingtime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }
        [XmlAttribute(AttributeName = "station")]
        public string Station { get; set; }
        [XmlAttribute(AttributeName = "shift")]
        public string Shift { get; set; }
        [XmlAttribute(AttributeName = "overtime")]
        public string Overtime { get; set; }
    }
}