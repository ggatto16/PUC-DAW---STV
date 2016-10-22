using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class UnidadeValidation
    {
        public static bool CanSee(Unidade uni, int Idusuario, IPrincipal User)
        {
            if (uni == null)
                throw new ApplicationException("Unidade não encontrada.");
            if (User.IsInRole("Admin"))
                return true;
            if (uni.Curso.IdusuarioInstrutor == Idusuario)
                return true;
            if (uni.DataAbertura > DateTime.Now)
                return false;
            if (!CommonValidation.CanSee(uni.Curso, Idusuario, User))
                return false;

            return true;
        }
    }
}