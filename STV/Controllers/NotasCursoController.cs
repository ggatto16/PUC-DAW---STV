using STV.Auth;
using STV.DAL;
using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace STV.Controllers
{
    public class NotasCursoController : Controller
    {

        private STVDbContext db = new STVDbContext();

        private Usuario UsuarioLogado;

        public NotasCursoController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }


        public async Task<ActionResult> Avaliar(int? Idcurso, int nota)
        {
            if (Idcurso == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NotaCurso NotaCurso = new NotaCurso
            {
                Idcurso = (int)Idcurso,
                Idusuario = UsuarioLogado.Idusuario,
                Pontos = nota
            };

            var notaAtual = await db.NotaCurso.FindAsync(UsuarioLogado.Idusuario, Idcurso);

            if(notaAtual != null)
            {
                db.NotaCurso.Attach(NotaCurso);
                db.Entry(NotaCurso).Property(q => q.Pontos).IsModified = true;
            }
            else
                db.NotaCurso.Add(NotaCurso);

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}
