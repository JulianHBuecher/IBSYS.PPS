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
        // Entity with Amount of Parts from Queue
        [JsonProperty("Required Parts out of Queue")]
        public int AdditionalParts { get; set; }
        [JsonProperty("Actual Stock")]
        public int Stock { get; set; }
        [JsonProperty("Gross Requirements for next Periods")]
        public double[] Requirements { get; set; }
        [JsonProperty("Order Quotient")]
        public double OrderQuotient { get; set; }
    }
}
