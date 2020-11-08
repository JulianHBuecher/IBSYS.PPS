using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IBSYS.PPS.Models.Capacity
{
    public class CapacityRequirement
    {
        public int processTime { get; set; }

        public int setupTime { get; set; }

        public int workStation { get; set; }

        public CapacityRequirement(int processTime, int setupTime, int workStation)
        {
            this.processTime = processTime;
            this.setupTime = setupTime;
            this.workStation = workStation;
        }
    }
}
