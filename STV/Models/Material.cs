using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace STV.Models
{

    public enum TipoMaterial
    {
        [Display(Name = "Arquivo de vídeo")]
        Video = 1,
        Arquivo = 2,
        [Display(Name = "Link para site ou arquivo")]
        Link = 3,
        Imagem = 4,
        [Display(Name = "URL para vídeo incorporado", Description = "URL para vídeo incorporado" )]
        Embedded = 5
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

        [Url]
        public string URL { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual Arquivo Arquivo { get; set; }

        public virtual ICollection<Usuario> UsuariosConsulta { get; set; }

    }
}