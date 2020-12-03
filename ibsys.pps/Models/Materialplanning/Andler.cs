using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace IBSYS.PPS.Models.Materialplanning
{
    public class Andler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [XmlIgnore]
        public int Id { get; set; }
        public int Konstante { get; set; }
        public int Jahresverbrauch { get; set; }
        public double BestellfixeKosten { get; set; }
        public double VariableBestellkosten { get; set; }
        public double Lagerkostensatz { get; set; }
        public double Lagerwert { get; set; }
        public double Result { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        [ForeignKey("OrderForKForeignKey")]
        public OrderForK OrderForKFK { get; set; }
    }
}
