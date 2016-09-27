using STV.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STV.ViewModels
{
    public class MaterialVM
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