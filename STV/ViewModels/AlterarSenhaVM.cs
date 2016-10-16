using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STV.ViewModels
{

    public class AlterarSenhaVM
    {
        public int Idusuario { get; set; }

        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public string Senha { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve conter de 6 a 20 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Senha")]
        public string SenhaDigitada { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve conter de 6 a 20 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Confirmar Senha")]
        public string SenhaDigitadaConfirmacao { get; set; }


    }

}