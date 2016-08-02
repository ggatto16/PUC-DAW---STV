namespace STV.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        public string Cpf { get; set; }

        [StringLength(60)]
        public string Nome { get; set; }

        [StringLength(70)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Senha { get; set; }

        public int Iddepartamento { get; set; }

        public bool Status { get; set; }

        public DateTime Stamp { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual ICollection<Curso> CursosGerenciaveis { get; set; }

        public virtual ICollection<Nota> Notas { get; set; }

        public virtual ICollection<Resposta> Respostas { get; set; }

    }
}
