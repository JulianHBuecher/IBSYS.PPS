using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Materialplanning
{
    public class OrderForK
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [XmlIgnore]
        public int Id { get; set; }
        public string PartName { get; set; }
        public string OrderQuantity { get; set; }
        // Status 4 - Eil-Bestellung
        // Status 5 - Normal-Bestellung
        public int OrderModus { get; set; }
    }
}
