using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STV.ViewModels
{

    public class UsuarioVM
    {
        public int Idusuario { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "CPF obrigatório")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        public string CpfSoNumeros
        {
            get
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                string ret = reg.Replace(Cpf, string.Empty);
                return ret;
            }
        }

        [StringLength(60, ErrorMessage = "Este campo suporta até 60 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public string Nome { get; set; }

        [StringLength(100, ErrorMessage = "Este campo suporta até 100 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public string Email { get; set; }

        [StringLength(500)]
        public string Senha { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve conter de 6 a 20 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Senha")]
        public string SenhaDigitada { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve conter de 6 a 20 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Confirmar Senha")]
        public string SenhaDigitadaConfirmacao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Departamento")]
        public int Iddepartamento { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Medalha> Medalhas { get; set; }

        public DateTime Stamp { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual ICollection<Curso> CursosGerenciaveis { get; set; }

        public virtual ICollection<Nota> Notas { get; set; }

        public virtual ICollection<Resposta> Respostas { get; set; }

        public virtual ICollection<NotaCurso> NotasCursos { get; set; }

        public virtual ICollection<Material> MateriaisConsultados { get; set; }

    }

}