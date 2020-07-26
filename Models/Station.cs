namespace IBSYS.PPS.Models
{
    public class Station
    {
        public string WorkplaceNumber { get; set; }
        public double CostsFirstShift { get; set; }
        public double CostsSecondShift { get; set; }
        public double CostsThirdShift { get; set; }
        public double CostsOverTime { get; set; }
    }
}