using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models
{
    public class LaborAndMachineCosts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Workplace { get; set; }
        public double LaborCostsFirstShift { get; set; }
        public double LaborCostsSecondShift { get; set; }
        public double LaborCostsThirdShift { get; set; }
        public double LaborCostsOvertime { get; set; }
        public double ProductiveMachineCosts { get; set; }
        public double IdleTimeMachineCosts { get; set; }
    }
}
