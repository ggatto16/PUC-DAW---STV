using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [StringLength(500)]
        public string Senha { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Departamento")]
        public int Iddepartamento { get; set; }

        public virtual ICollection<Medalha> Medalhas { get; set; }

        [Display(Name = "Data de Criação")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public DateTime Stamp { get; set; }

        public virtual Departamento Departamento { get; set; }

        [Display(Name = "Perfil")]
        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual ICollection<Curso> CursosGerenciaveis { get; set; }

        public virtual ICollection<Nota> Notas { get; set; }

        public virtual ICollection<Resposta> Respostas { get; set; }

        public virtual ICollection<NotaCurso> NotasCursos { get; set; }

        public virtual ICollection<Material> MateriaisConsultados { get; set; }

    }

}