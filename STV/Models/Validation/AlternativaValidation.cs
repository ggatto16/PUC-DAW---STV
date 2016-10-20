using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class AlternativaValidation
    {
        public static void CanEdit(Alternativa alt, int Idusuario, IPrincipal User)
        {
            if (alt == null)
                throw new KeyNotFoundException("Ops! Alternativa não encontrada.");
            if (CommonValidation.Encerrada(alt.Questao.Atividade.DataEncerramento))
                throw new ApplicationException("Atividade encerrada. Não pode ser alterada.");
            if (CommonValidation.EmAberto(alt.Questao.Atividade.DataAbertura, alt.Questao.Atividade.DataEncerramento))
                throw new ApplicationException("Atividade em aberta e publicada. Não pode ser alterada.");

            CommonValidation.ChecarUsuarioAutorizado(alt.Questao.Atividade.Unidade.Curso.IdusuarioInstrutor, Idusuario, User);

        }
    }
}