using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class CommonValidation
    {
        public static bool CanSee(Curso curso, int Idusuario, IPrincipal User)
        {
            if (User.IsInRole("Admin") || curso.Usuarios.Where(u => u.Idusuario == Idusuario).Count() > 0 || curso.IdusuarioInstrutor == Idusuario)
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

        public static void ChecarUsuarioAutorizado(int IdusuarioInstrutor, int Idusuario, IPrincipal User)
        {
            if (IdusuarioInstrutor != Idusuario && !User.IsInRole("Admin"))
                throw new UnauthorizedAccessException("Não autorizado");
        }
    }
}