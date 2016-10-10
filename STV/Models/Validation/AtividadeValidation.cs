using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class AtividadeValidation
    {
        public static void CanEdit(Atividade atv)
        {
            if (atv == null)
                throw new ApplicationException("Ops! Atividade não encontrada.");
            if (CommonValidation.Encerrada(atv.DataEncerramento))
                throw new ApplicationException("Atividade encerrada. Não pode ser alterada.");
            if (CommonValidation.EmAberto(atv.DataAbertura, atv.DataEncerramento))
                throw new ApplicationException("Atividade em aberto e publicada. Não pode ser alterada.");

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

        public static void CanDelete(Atividade atv)
        {
            if (atv == null)
                throw new ApplicationException("Ops! Atividade não encontrada.");
            if (CommonValidation.Encerrada(atv.DataEncerramento))
                throw new ApplicationException("Atividade encerrada. Não pode ser excluída.");
            if (CommonValidation.EmAberto(atv.DataAbertura, atv.DataEncerramento))
                throw new ApplicationException("Atividade em aberto e publicada. Não pode ser excluída.");
            if(atv.Questoes != null && atv.Questoes.Count() > 0)
                throw new ApplicationException("Atividade contém questões, por isso não pode ser excluída.");
            if (atv.Notas != null && atv.Notas.Count() > 0)
                throw new ApplicationException("Atividade contém notas, por isso não pode ser excluída.");

            return;
        }
    }
}