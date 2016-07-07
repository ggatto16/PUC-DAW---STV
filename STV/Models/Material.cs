using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using STV.Enum;

namespace STV.Models
{
    [Table("Material")]
    public partial class Material
    {
        public int Idmaterial { get; set; }

        public int Idunidade { get; set; }

        [Required]
        [StringLength(40)]
        public string Descricao { get; set; }

        [Required]
        public int Tipo { get; set; }

        public virtual Unidade Unidade { get; set; }

    }
}