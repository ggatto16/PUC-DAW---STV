using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class MaterialValidation
    {

        public static void CanEdit(Unidade uni, int Idusuario, IPrincipal User)
        {
            if (uni == null)
                throw new KeyNotFoundException("Unidade não encontrada.");
            if (uni.Encerrada)
                throw new ApplicationException("Este material pertence à uma unidade encerrada, por isso não pode ser alterado.");
            if (uni.Curso.Encerrado)
                throw new ApplicationException("Este material pertence à um curso encerrado, por isso não pode ser alterado.");

            CommonValidation.ChecarUsuarioAutorizado(uni.Curso.IdusuarioInstrutor, Idusuario, User);

            return;
        }

        public static void CanDelete(Unidade uni, int Idusuario, IPrincipal User)
        {
            CanEdit(uni, Idusuario, User);
        }
    }
}