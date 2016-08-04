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

namespace STV.Controllers
{
    public class AlternativasController : Controller
    {
        private STVDbContext db = new STVDbContext();

        // GET: Alternativas
        public async Task<ActionResult> Index()
        {
            var alternativa = db.Alternativa.Include(a => a.Questao);
            return View(await alternativa.ToListAsync());
        }

        // GET: Alternativas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alternativa alternativa = await db.Alternativa.FindAsync(id);
            if (alternativa == null)
            {
                return HttpNotFound();
            }
            return View(alternativa);
        }

        // GET: Alternativas/Create
        public async Task<ActionResult> Create(int Idquestao)
        {
            ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao");
            Alternativa alternativa = new Alternativa();
            alternativa.Idquestao = Idquestao;
            alternativa.Questao = await db.Questao.FindAsync(Idquestao);
            return View(alternativa);
        }

        // POST: Alternativas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idalternativa,Idquestao,Descricao,IsCorreta")] Alternativa alternativa)
        {
            if (ModelState.IsValid)
            {
                var questao = await db.Questao.FindAsync(alternativa.Idquestao);
                questao.IdalternativaCorreta = alternativa.Idalternativa;
                db.Questao.Attach(questao);
                db.Entry(questao).Property(q => q.IdalternativaCorreta).IsModified = true;

                db.Alternativa.Add(alternativa);
                await db.SaveChangesAsync();
                return VoltarParaListagem(alternativa);
            }

            ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao", alternativa.Idquestao);
            return View(alternativa);
        }

        // GET: Alternativas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alternativa alternativa = await db.Alternativa.FindAsync(id);
            if (alternativa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao", alternativa.Idquestao);
            return View(alternativa);
        }

        // POST: Alternativas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idalternativa,Idquestao,Descricao")] Alternativa alternativa)
        {
            if (ModelState.IsValid)
            {
                var questao = await db.Questao.FindAsync(alternativa.Idquestao);
                questao.IdalternativaCorreta = alternativa.Idalternativa;
                db.Questao.Attach(questao);
                db.Entry(questao).Property(q => q.IdalternativaCorreta).IsModified = true;

                db.Entry(alternativa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return VoltarParaListagem(alternativa);
            }
            ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao", alternativa.Idquestao);
            return View(alternativa);
        }

        // GET: Alternativas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alternativa alternativa = await db.Alternativa.FindAsync(id);
            if (alternativa == null)
            {
                return HttpNotFound();
            }
            return View(alternativa);
        }

        // POST: Alternativas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Alternativa alternativa = await db.Alternativa.FindAsync(id);
            db.Alternativa.Remove(alternativa);
            await db.SaveChangesAsync();
            return VoltarParaListagem(alternativa);
        }

        //Retorna para a tela principal da Atividade
        private RedirectToRouteResult VoltarParaListagem (Alternativa alternativa)
        {
            try
            {
                Questao questao = db.Questao.Find(alternativa.Idquestao);
                return RedirectToAction("Details", "Atividades", new { id = questao.Idatividade, Idquestao = alternativa.Idquestao });
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
