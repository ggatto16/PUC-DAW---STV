using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class AtividadeValidation
    {
        public static void ChecarUsuarioAutorizado(int IdusuarioInstrutor, int Idusuario, IPrincipal User)
        {
            if (IdusuarioInstrutor != Idusuario && !User.IsInRole("Admin"))
                throw new UnauthorizedAccessException("Não autorizado");
        }

        public static void CanEdit(Atividade atv, int Idusuario, IPrincipal User)
        {
            if (atv == null)
                throw new KeyNotFoundException("Atividade não encontrada.");
            if (atv.DataEncerramento != DateTime.MinValue && CommonValidation.Encerrada(atv.DataEncerramento))
                throw new ApplicationException("Atividade encerrada. Não pode ser alterada.");
            if (atv.DataAbertura != DateTime.MinValue && CommonValidation.EmAberto(atv.DataAbertura, atv.DataEncerramento))
                throw new ApplicationException("Atividade está aberta e publicada. Não pode ser alterada.");
            if (atv.Unidade.Encerrada)
                throw new ApplicationException("A unidade desta atividade está encerrada, por isso não pode ser alterada.");
            if (atv.Unidade.Curso.Encerrado)
                throw new ApplicationException("O curso desta atividade está encerrado, por isso não pode ser alterada.");

            ChecarUsuarioAutorizado(atv.Unidade.Curso.IdusuarioInstrutor, Idusuario, User);

            return;
        }

        public static bool CanDo(Atividade atv, int Idusuario, IPrincipal User)
        {
            if (atv == null)
                throw new ApplicationException("Ops! Atividade não encontrada.");

            if (!CommonValidation.CanSee(atv.Unidade.Curso, Idusuario, User)
                    || CommonValidation.Encerrada(atv.DataEncerramento))
                return false;

            return true;
        }

        public static void CanDelete(Atividade atv, int Idusuario, IPrincipal User)
        {
            CanEdit(atv, Idusuario, User);
            if (atv.Questoes != null && atv.Questoes.Count() > 0)
                throw new ApplicationException("Atividade contém questões, por isso não pode ser excluída.");
            if (atv.Notas != null && atv.Notas.Count() > 0)
                throw new ApplicationException("Atividade contém notas, por isso não pode ser excluída.");
        }
    }
}