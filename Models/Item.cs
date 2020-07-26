using System.Xml.Serialization;

namespace IBSYS.PPS.Models
{
    public class SellWishItem
    {
        [XmlAttribute]
        public string article { get; set; }
        [XmlAttribute]
        public int quantity { get; set; }
    }

    public class SellDirectItem : SellWishItem
    {
        [XmlAttribute]
        public double price { get; set; }
        [XmlAttribute]
        public double penalty { get; set; }
    }
}