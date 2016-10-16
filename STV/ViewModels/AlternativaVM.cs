using STV.Models;
using System.ComponentModel.DataAnnotations;

namespace STV.ViewModels
{
    public class AlternativaVM
    {
        public int Idalternativa { get; set; }

        public int Idquestao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [StringLength(1000, ErrorMessage = "Este campo suporta até 1000 caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [StringLength(1000, ErrorMessage = "Este campo suporta até 1000 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Justificativa { get; set; }

        [Display(Name = "Questão")]
        public virtual Questao Questao { get; set; }

        public bool IsCorreta { get; set; }
    }
}