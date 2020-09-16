using System.Xml.Serialization;

namespace IBSYS.PPS.Models
{
    [XmlRoot("input", Namespace = "",IsNullable = false)]
    public class Input
    {
        [XmlElement(ElementName = "user", IsNullable = false)]
        public User User;
        [XmlElement(ElementName = "qualitycontrol", IsNullable = false)]
        public Qualitycontrol Qualitycontrol;
        [XmlArray("sellwish")] 
        [XmlArrayItem("item")]
        public SellWishItem[] PrognosedItems;
        [XmlArray("selldirect")] 
        [XmlArrayItem("item")]
        public SellDirectItem[] DirectSellItems;
        [XmlArray("orderlist")]
        [XmlArrayItem("order")] 
        public Order[] Orders;
        [XmlArray("productionlist")]
        [XmlArrayItem("production")] 
        public Production[] Productions;
        [XmlArray("workingtimelist")]
        [XmlArrayItem("workingtime")]
        public Workingtime[] Workingtimes;
    }
}