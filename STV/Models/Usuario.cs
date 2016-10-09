using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STV.Models
{
    public enum UserRoles
    {
        Administrador = 1,
        Padrao = 2 
    };


    [Table("Usuario")]
    public partial class Usuario
    {

        public int Idusuario { get; set; }

        [StringLength(15)]
        [Required]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [StringLength(60)]
        [Required]
        public string Nome { get; set; }

        [StringLength(70)]
        public string Email { get; set; }

        [StringLength(500)]
        [Required]
        public string Senha { get; set; }

        public int? Iddepartamento { get; set; }

        public virtual ICollection<Medalha> Medalhas { get; set; }

        [Display(Name = "Data de Criação")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
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
