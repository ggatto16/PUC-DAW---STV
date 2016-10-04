using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Utils
{
    public class Autorizacao
    { 
        public static bool UsuarioInscrito(ICollection<Usuario> UsuariosInscritos, int Idusuario, IPrincipal User)
        {
            if (UsuariosInscritos.Where(u => u.Idusuario == Idusuario).Count() > 0 || User.IsInRole("Admin"))
                return true;
            else
                return false;
        }
    }
}