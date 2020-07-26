namespace IBSYS.PPS.Models
{
    public class Article
    {
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string[] WhereUsed { get; set; }
        public double ItemValue { get; set; }
        public int QuantityInStock { get; set; }
    }
}