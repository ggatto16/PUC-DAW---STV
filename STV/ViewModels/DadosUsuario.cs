using STV.Models;
using System.Collections.Generic;

namespace STV.ViewModels
{
    public class DadosUsuario
    {
        public int Idusuario { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Iddepartamento { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}