using STV.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STV.ViewModels
{
    public class MaterialVM
    {
        public int Idmaterial { get; set; }

        public int Idunidade { get; set; }

        [Display(Name = "Descrição")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [StringLength(70, ErrorMessage = "Este campo suporta até 70 caracteres")]
        public string Descricao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public TipoMaterial Tipo { get; set; }

        [Url(ErrorMessage = "URL inválida")]
        [StringLength(300, ErrorMessage = "Este campo suporta até 300 caracteres. Utilize um encurtador de URL se necessário.")]
        public string URL { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual Arquivo Arquivo { get; set; }

        public virtual ICollection<Usuario> UsuariosConsulta { get; set; }

        public int Idcurso { get; set; }

    }
}