using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class CursoValidation
    {
        public static bool EmAberto(Curso curso)
        {
            return curso.DataInicial <= DateTime.Now && !curso.Encerrado;
        }

        public static void CanDelete(Curso curso)
        {
            if (curso == null)
                throw new ApplicationException("Curso não encontrado.");
            if (curso.Encerrado)
                throw new ApplicationException("Curso encerrado. Não pode ser excluído.");
            if (EmAberto(curso))
                throw new ApplicationException("Curso em andamento. Não pode ser excluído.");
            if (curso.Unidades != null && curso.Unidades.Count() > 0)
                throw new ApplicationException("Curso contém unidades, por isso não pode ser excluído.");
            if (curso.Usuarios != null && curso.Usuarios.Count() > 0)
                throw new ApplicationException("Curso contém incritos, por isso não pode ser excluído.");

            return;
        }

        public static void CanEdit(Curso curso,int Idusuario, IPrincipal User)
        {
            if (curso == null)
                throw new HttpRequestValidationException("Curso não encontrado.");
            if (curso.Encerrado)
                throw new ApplicationException("Curso encerrado. Não pode ser alterado.");
            if (!User.IsInRole("Admin") && curso.IdusuarioInstrutor != Idusuario)
                throw new UnauthorizedAccessException("Não Autorizado");
        }
    }
}