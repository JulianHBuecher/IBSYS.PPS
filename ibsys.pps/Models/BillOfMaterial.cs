using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models
{
    public class BillOfMaterial
    {
        [Key]
        public string ProductName { get; set; }
        public List<Material> RequiredMaterials { get; set; }
    }
}
