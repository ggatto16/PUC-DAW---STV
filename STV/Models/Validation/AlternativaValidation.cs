using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STV.Models.Validation
{
    public static class AlternativaValidation
    {
        public static bool CanEdit(Alternativa alt)
        {
            if (alt == null)
                throw new ApplicationException("Ops! Alternativa não encontrada.");
            if (CommonValidation.Encerrada(alt.Questao.Atividade.DataEncerramento))
                throw new ApplicationException("Atividade encerrada. Não pode ser alterada.");
            if (CommonValidation.EmAberto(alt.Questao.Atividade.DataAbertura, alt.Questao.Atividade.DataEncerramento))
                throw new ApplicationException("Atividade em aberto e publicada. Não pode ser alterada.");

            return true;
        }
    }
}