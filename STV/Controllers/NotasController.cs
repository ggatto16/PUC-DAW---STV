using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using STV.Models;
using STV.DAL;
using Microsoft.Owin;
using STV.Auth;

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

            ViewBag.Unidade = await db.Unidade.Where(u => u.Idunidade == Idunidade)
                .Select(u => u.Titulo).SingleAsync();
                
            return View(notas);
        }
    }
}