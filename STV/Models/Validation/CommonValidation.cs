using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class CommonValidation
    {
        public static bool UsuarioEstaInscrito(ICollection<Usuario> UsuariosInscritos, int Idusuario, IPrincipal User)
        {
            if (UsuariosInscritos.Where(u => u.Idusuario == Idusuario).Count() > 0 || User.IsInRole("Admin"))
                return true;
            else
                return false;
        }

        public static bool Encerrada(DateTime DataEncerramento)
        {
            return DataEncerramento < DateTime.Now;
        }

        public static bool EmAberto(DateTime DataAbertura, DateTime DataEncerramento)
        {
            return DataAbertura < DateTime.Now && DataEncerramento >= DateTime.Now;
        }
    }
}