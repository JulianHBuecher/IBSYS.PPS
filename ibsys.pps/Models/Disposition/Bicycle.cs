using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IBSYS.PPS.Models.Disposition
{
    public class Bicycle
    {
        public List<BicyclePart> parts { get; set; }
    }

    public class BicyclePart
    {
        public string name { get; set; }
        [JsonPropertyName("Orders From Queue (previous part)")]
        public string? ordersInQueueInherit { get; set; }
        public string plannedWarehouseFollowing { get; set; }
        public string warehouseStockPassed { get; set; }
        [JsonPropertyName("Orders From Queue")]
        public string ordersInQueueOwn { get; set; }
        [JsonPropertyName("Work In Progress")]
        public string wip { get; set; }
        public string quantity { get; set; }

        //public BicyclePart(string name, string ordersInQueueInherit = null, string plannedWareHouseFollowing = null,
        //    string wareHouseStockPassed = null, string ordersInQueueOwn = null, string wip = null, string quantity = null)
        //{
        //    this.name = name;
        //    this.ordersInQueueInherit = ordersInQueueInherit;
        //    this.plannedWarehouseFollowing = plannedWarehouseFollowing;
        //    this.warehouseStockPassed = warehouseStockPassed;
        //    this.ordersInQueueOwn = ordersInQueueOwn;
        //    this.wip = wip;
        //    this.quantity = quantity;
        //}
    }
}
