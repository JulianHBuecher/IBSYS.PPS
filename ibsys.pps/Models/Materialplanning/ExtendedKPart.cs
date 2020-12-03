using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models.Materialplanning
{
    public class ExtendedKPart
    {
        [JsonProperty("Item Number")]
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        [JsonProperty("Discount Quantity")]
        public int DiscountQuantity { get; set;}
        [JsonProperty("Order Costs")]
        public double OrderCosts { get; set; }
        [JsonProperty("Required Parts out of Queue")]
        public int AdditionalParts { get; set; }
        [JsonProperty("Actual Stock")]
        public int Stock { get; set; }
        [JsonProperty("Gross Requirements for next Periods")]
        public double[] Requirements { get; set; }
        [JsonProperty("Order Quotient")]
        public double OrderQuotient { get; set; }
        [JsonProperty("Optimal Order Quantity")]
        public Andler OptimalOrderQuantity { get; set; }
    }
}
