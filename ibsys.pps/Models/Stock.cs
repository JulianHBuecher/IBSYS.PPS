using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBSYS.PPS.Models
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
        public double ItemValue { get; set; }
        public int QuantityInStock { get; set; }
    }

    public class SelfProductionItems : Stock
    {

    }

    public class PurchasedItems : Stock
    {
        public int DiscountQuantity { get; set; }
        public double OrderCosts { get; set; }
        public double ProcureLeadTime { get; set; }
        public double Deviation { get; set; }
    }
}
