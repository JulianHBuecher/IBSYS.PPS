using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Input
{
    public class SellWishItem
    { 
        [XmlAttribute("article")]
        public string Article { get; set; }
        [XmlAttribute("quantity")]
        public int Quantity { get; set; }
    }

    public class SellDirectItem : SellWishItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }
        [XmlAttribute("price")]
        public double Price { get; set; }
        [XmlAttribute("penalty")]
        public double Penalty { get; set; }
    }
}