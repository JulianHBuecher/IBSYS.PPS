using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models.Disposition
{
    public class PlannedWarehouseStock
    {
        public string Part { get; set; }
        public int Amount { get; set; }

        public PlannedWarehouseStock(string part, int amount)
        {
            Part = part;
            Amount = amount;
        }
    }
}
