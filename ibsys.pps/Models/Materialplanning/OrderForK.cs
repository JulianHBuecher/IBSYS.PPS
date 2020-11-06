namespace IBSYS.PPS.Models.Materialplanning
{
    public class OrderForK
    {
        public string PartName { get; set; }
        public string OrderQuantity { get; set; }
        // Status 4 - Eil-Bestellung
        // Status 5 - Normal-Bestellung
        public int OrderModus { get; set; }
    }
}
