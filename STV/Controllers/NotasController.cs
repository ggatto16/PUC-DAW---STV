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
                .Where(n => n.Idusuario == UsuarioLogado.Idusuario && n.Atividade.Idunidade == Idunidade && n.Atividade.Dtencerramento < DateTime.Now);


            var usuario = await db.Usuario.FindAsync(UsuarioLogado.Idusuario);
            List<Medalha> medalhas = await db.Medalha.ToListAsync();

            //Medalha sortudo = medalhas.Where(x => x.Idmedalha == (int)Medalhas.Sortudo).FirstOrDefault();

            foreach (Medalha m in medalhas)
            {
                switch ((Medalhas)m.Idmedalha)
                {
                    case Medalhas.Sortudo:
                        foreach (var nota in notas)
                        {
                            if (nota.Atividade.Valor == nota.Pontos && !usuario.Medalhas.Contains(m))
                                usuario.Medalhas.Add(m);
                        }
                        break;

                    case Medalhas.Nerd:
                            
                        break;

                    default:
                        break;
                }
            }

            ViewBag.Unidade = await db.Unidade.Where(u => u.Idunidade == Idunidade)
                .Select(u => u.Titulo).SingleAsync();
                
            return View(notas);
        }
    }
}