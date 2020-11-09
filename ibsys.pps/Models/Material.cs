using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models
{
    public class Material
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public string MaterialName { get; set; }
        public int QuantityNeeded { get; set; }
        public List<Material> MaterialNeeded { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        [ForeignKey("ParentMaterialId")]
        public Material ParentMaterial { get; set; }

        public Material()
        {
            ID = Guid.NewGuid();
        }
    }
}
