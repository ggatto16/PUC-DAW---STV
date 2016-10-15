using STV.Auth;
using STV.DAL;
using STV.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STV.Controllers
{
    [Authorize]
    public class NotasController : Controller
    {
        private STVDbContext db = new STVDbContext();

        private Usuario UsuarioLogado;
        public NotasController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }

        // GET: Notas
        public async Task<ActionResult> MinhasNotas(int? Idunidade)
        {
            if (Idunidade == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var notas = db.Nota.Include(n => n.Atividade)
                .Where(n => n.Idusuario == UsuarioLogado.Idusuario && n.Atividade.Idunidade == Idunidade && n.Atividade.DataEncerramento < DateTime.Now);

            //ViewBag.Unidade = await db.Unidade.Where(u => u.Idunidade == Idunidade)
            //    .Select(u => u.Titulo).SingleAsync();

           var objUnidade = await db.Unidade.Where(u => u.Idunidade == Idunidade)
                .Select(u => new
                {
                    Idunidade = u.Idunidade,
                    Titulo = u.Titulo,
                    Idcurso = u.Idcurso,
                    Curso = u.Curso.Titulo
                }).SingleAsync();

            ViewBag.Idunidade = objUnidade.Idunidade;
            ViewBag.Titulo = objUnidade.Titulo;
            ViewBag.Idcurso = objUnidade.Idcurso;
            ViewBag.Curso = objUnidade.Curso;

            return View(notas);
        }
    }
}