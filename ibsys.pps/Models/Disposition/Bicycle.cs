using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Disposition
{
    public class Bicycle
    {
        public List<BicyclePart> Parts { get; set; }
    }

    public class BicyclePart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [XmlIgnore]
        [JsonIgnore]
        public int ID { get; set; }
        public string Name { get; set; }
        [JsonProperty("Orders From Queue (previous part)")]
        public string? OrdersInQueueInherit { get; set; }
        public string PlannedWarehouseFollowing { get; set; }
        public string WarehouseStockPassed { get; set; }
        [JsonProperty("Orders From Queue")]
        public string OrdersInQueueOwn { get; set; }
        [JsonProperty("Work In Progress")]
        public string Wip { get; set; }
        public string Quantity { get; set; }
    }

    public class OptimizedPart : BicyclePart
    {
        [JsonProperty("Optimized Order")]
        public int Optimized { get; set; }
    }
}
