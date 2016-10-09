using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STV.ViewModels
{
    public class Anonymous
    {

        [StringLength(15)]
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

        [StringLength(500)]
        public string Senha { get; set; }

    }
}