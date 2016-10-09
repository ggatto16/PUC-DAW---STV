using System.ComponentModel.DataAnnotations;

namespace STV.ViewModels
{
    public class Anonymous
    {

        [StringLength(15, ErrorMessage = "Este campo suporta até 15 caracteres")]
        [Required(ErrorMessage = "CPF obrigatório")]
        public string UserId { get; set; }

        public string CpfLoginSoNumeros
        {
            get
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                string ret = reg.Replace(UserId, string.Empty);
                return ret;
            }
        }

        [StringLength(500, ErrorMessage = "Este campo suporta até 500 caracteres")]
        [Required(ErrorMessage = "Senha obrigatória")]
        public string Senha { get; set; }

    }
}