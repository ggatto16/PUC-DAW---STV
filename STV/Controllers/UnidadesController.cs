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
                //.Include(u => u.Atividades)
                //.Include(u => u.Materiais)
                .Where(u => u.Idunidade == idunidade)
                .SingleOrDefaultAsync();

            var atividades = db.Atividade
                .Where(a => a.Idunidade == unidade.Idunidade && a.Questoes.Count() > 0).ToList();

            unidade.Atividades = atividades;

            foreach (var atividade in unidade.Atividades)
            {
                int respondidas = await db.Resposta
                    .Where(r => r.Idusuario == UsuarioLogado.Idusuario && r.Questao.Idatividade == atividade.Idatividade)
                    .CountAsync();

                atividade.Realizado += atividade.PorcentagemQuestao * respondidas;

                int nota = db.Nota.Where(n => n.Idusuario == UsuarioLogado.Idusuario && n.Idatividade == atividade.Idatividade).Count();
                if (nota > 0)
                    atividade.IsFinalizada = true;
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

        private bool Autorizarado (int? Idcurso)
        {
            var curso = db.Curso
                .Where(c => c.IdusuarioInstrutor == UsuarioLogado.Idusuario && c.Idcurso == Idcurso)
                .SingleOrDefault();

            return curso == null ? false : true;
        }

        public ActionResult NaoAutorizado()
        {
            return View();
        }

        // GET: Unidades/Create
        public ActionResult Create(int? Idcurso)
        {
            if (Idcurso == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!Autorizarado(Idcurso)) return View("NaoAutorizado");

            ViewBag.Idcurso = new SelectList(db.Curso, "Idcurso", "Titulo");
            return View();
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idunidade,Idcurso,Titulo,Dtabertura,Status")] Unidade unidade)
        {
            if (!Autorizarado(unidade.Idcurso)) return View("NaoAutorizado");

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Unidade unidade = await db.Unidade.FindAsync(id);
            if (unidade == null)
                return HttpNotFound();

            Autorizarado(unidade.Idcurso);
            if (!Autorizarado(unidade.Idcurso)) return View("NaoAutorizado");

            ViewBag.Idcurso = new SelectList(db.Curso, "Idcurso", "Titulo");
            return View(unidade);
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idunidade,Idcurso,Titulo,Dtabertura,Status")] Unidade unidade)
        {
            if (!Autorizarado(unidade.Idcurso)) return View("NaoAutorizado");

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Unidade unidade = await db.Unidade.FindAsync(id);

            if (unidade == null)
                return HttpNotFound();

            if (!Autorizarado(unidade.Idcurso)) return View("NaoAutorizado");

            return View(unidade);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Unidade unidade = await db.Unidade.FindAsync(id);
            if (!Autorizarado(unidade.Idcurso)) return View("NaoAutorizado");
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
