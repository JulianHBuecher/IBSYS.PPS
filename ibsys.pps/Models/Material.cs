using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models
{
    public class Material
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MaterialName { get; set; }
        public int QuantityNeeded { get; set; }
        public List<Material> MaterialNeeded { get; set; }
        [JsonIgnore]
        [ForeignKey("ParentMaterialId")]
        public Material ParentMaterial { get; set; }
    }
}
