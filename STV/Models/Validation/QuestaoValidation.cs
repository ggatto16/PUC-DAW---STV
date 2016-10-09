using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace STV.Models.Validation
{
    public static class QuestaoValidation
    {
        public static bool CanSee(Questao que, int Idusuario, IPrincipal User)
        {
            if (que == null)
                throw new ApplicationException("Questão não encontrada.");
            if (User.IsInRole("Admin"))
                return true;
            if (que.Atividade.Unidade.Curso.IdusuarioInstrutor == Idusuario)
                return true;
            if (!CommonValidation.EmAberto(que.Atividade.DataAbertura, que.Atividade.DataEncerramento))
                return false;
            if (!CommonValidation.UsuarioEstaInscrito(que.Atividade.Unidade.Curso.Usuarios, Idusuario, User))
                return false;

            return true;
        }
    }
}