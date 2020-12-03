using Newtonsoft.Json;

namespace IBSYS.PPS.Models.Capacity
{
    public class CapacityRequirementExtended
    {
        [JsonProperty("Workstation ID")]
        public int Workstation { get; set; }
        [JsonProperty("Required Time from Queue")]
        public int TimeFromWaitinglist { get; set; }
        [JsonProperty("Required Time from WIP")]
        public int TimeFromWiP { get; set; }
        [JsonProperty("Setup Events (from last period)")]
        public int SetupEvents { get; set; }
        [JsonProperty("Setup-Time Calculated")]
        public int SetupTime { get; set; }
        [JsonProperty("Required Capacity")]
        public int RequiredCapacity { get; set; }
    }
}
