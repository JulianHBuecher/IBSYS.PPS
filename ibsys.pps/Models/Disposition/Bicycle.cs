using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models.Disposition
{
    public class Bicycle
    {
        public List<BicyclePart> parts { get; set; }
    }

    public class BicyclePart
    {
        public string name { get; set; }
        public string quantity { get; set; }

        public BicyclePart(string name, string quantity)
        {
            this.name = name;
            this.quantity = quantity;
        }
    }
}
