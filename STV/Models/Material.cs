using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace STV.Models
{

    public enum TipoMaterial
    {
        Nenhum = 0,
        Video = 1,
        Arquivo = 2,
        Link = 3,
        Imagem = 4
    };

    [Table("Material")]
    public partial class Material
    {
        public int Idmaterial { get; set; }

        public int Idunidade { get; set; }

        [Required]
        [StringLength(40)]
        public string Descricao { get; set; }

        [Required]
        public TipoMaterial Tipo { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual Arquivo Arquivo { get; set; }

    }
}