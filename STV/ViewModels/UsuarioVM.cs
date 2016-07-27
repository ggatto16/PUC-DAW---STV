using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STV.ViewModels
{
    public class UsuarioVM
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

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Role> RolesDisponiveis { get; set; }
    }
}