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
using STV.Auth;

namespace STV.Controllers
{
    public class UnidadesController : Controller
    {
        private STVDbContext db = new STVDbContext();

        private Usuario UsuarioLogado;

        public UnidadesController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }

        // GET: Unidades
        public async Task<ActionResult> Index(int idcurso = 0)
        {
            if (idcurso != 0)
            {
                var unidades = from u in db.Unidade where u.Curso.Idcurso == idcurso select u;
                return View(await unidades.ToListAsync());
            }
            else
            {
                var unidades = db.Unidade.Include(u => u.Curso);
                return View(await unidades.ToListAsync());
            }
        }

        // GET: Conteúdo da Unidade
        public async Task<ActionResult> CarregarConteudo(int? idunidade)
        {
            if (idunidade == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var unidade = await db.Unidade
                .Include(u => u.Atividades)
                .Include(u => u.Materiais)
                .SingleOrDefaultAsync();

            //var unidade = await db.Unidade.FindAsync(idunidade);

            //var materiais = from m in db.Material where m.Idunidade == idunidade select m;
            //unidade.Materiais = await materiais.ToListAsync();

            //var atividades = from a in db.Atividade where a.Idunidade == idunidade select a;
            //unidade.Atividades = await atividades.ToListAsync();

            foreach (var atividade in unidade.Atividades)
            {
                int respondidas = await db.Resposta
                    .Where(r => r.Idusuario == UsuarioLogado.Idusuario && r.Questao.Idatividade == atividade.Idatividade)
                    .CountAsync();

                atividade.Realizado += atividade.PorcentagemQuestao * respondidas;
            }

            return PartialView("Conteudo", unidade);
           
        }

        // GET: Unidades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = await db.Unidade.FindAsync(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(unidade);
        }

        // GET: Unidades/Create
        public ActionResult Create()
        {
            ViewBag.Idcurso = new SelectList(db.Curso, "Idcurso", "Titulo");
            return View();
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idunidade,Idcurso,Titulo,Dtabertura,Status,Stamp")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                unidade.Stamp = DateTime.Now;
                db.Unidade.Add(unidade);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(unidade);
        }

        // GET: Unidades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = await db.Unidade.FindAsync(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idcurso = new SelectList(db.Curso, "Idcurso", "Titulo");
            return View(unidade);
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idunidade,Idcurso,Titulo,Dtabertura,Status,Stamp")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                unidade.Stamp = DateTime.Now;
                db.Entry(unidade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(unidade);
        }

        // GET: Unidades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = await db.Unidade.FindAsync(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(unidade);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Unidade unidade = await db.Unidade.FindAsync(id);
            db.Unidade.Remove(unidade);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
