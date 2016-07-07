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

namespace STV.Controllers
{
    public class AtividadesController : Controller
    {
        private ModeloDados db = new ModeloDados();

        // GET: Atividades
        public async Task<ActionResult> Index(int idunidade = 0)
        {
            if (idunidade != 0)
            {
                var atividades = from a in db.Atividade where a.Unidade.Idunidade == idunidade select a;
                return PartialView(await atividades.ToListAsync());
            }
            else
            {
                var atividades = db.Atividade.Include(m => m.Unidade);
                return PartialView(await atividades.ToListAsync());
            }
        }

        // GET: Atividades/Details/5
        public async Task<ActionResult> Details(int? id, int? Idquestao)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        
            Atividade atividade = await db.Atividade.FindAsync(id);

            if (atividade == null)
            {
                return HttpNotFound();
            }

            ViewBag.QuestaoSelecionada = Idquestao;

            return View(atividade);
        }

        // GET: Atividades/Create
        public ActionResult Create(int Idunidade)
        {
            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo");
            Atividade atividade = new Atividade();
            atividade.Idunidade = Idunidade;
            return View(atividade);
        }

        // POST: Atividades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idatividade,Idunidade,Descricao,Valor")] Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                db.Atividade.Add(atividade);
                await db.SaveChangesAsync();
                return VoltarParaListagem(atividade);
            }

            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo", atividade.Idunidade);
            return View(atividade);
        }

        // GET: Atividades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atividade atividade = await db.Atividade.FindAsync(id);
            if (atividade == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo", atividade.Idunidade);
            return View(atividade);
        }

        // POST: Atividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idatividade,Idunidade,Descricao,Valor")] Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atividade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return VoltarParaListagem(atividade);
            }
            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo", atividade.Idunidade);
            return View(atividade);
        }

        // GET: Atividades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atividade atividade = await db.Atividade.FindAsync(id);
            if (atividade == null)
            {
                return HttpNotFound();
            }
            return View(atividade);
        }

        // POST: Atividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Atividade atividade = await db.Atividade.FindAsync(id);
            db.Atividade.Remove(atividade);
            await db.SaveChangesAsync();
            return VoltarParaListagem(atividade);
        }

        //Retorna para a tela principal do Curso
        private RedirectToRouteResult VoltarParaListagem(Atividade atividade)
        {
            try
            {
                Unidade unidade = db.Unidade.Find(atividade.Idunidade);
                return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso, Idunidade = atividade.Idunidade });
            }
            catch (Exception)
            {
                throw;
            }
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
