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
    public class QuestoesController : Controller
    {
        private STVDbContext db = new STVDbContext();

        // GET: Questoes
        public async Task<ActionResult> Index()
        {
            var questao = db.Questao.Include(q => q.Alternativa).Include(q => q.Atividade);
            return View(await questao.ToListAsync());
        }

        // GET: Conteúdo da Unidade
        public async Task<ActionResult> CarregarAlternativas(int? Idquestao)
        {
            if (Idquestao == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var questao = await db.Questao.FindAsync(Idquestao);

            var alternativas = from a in db.Alternativa where a.Idquestao == Idquestao select a;
            questao.Alternativas = await alternativas.ToListAsync();

            return PartialView("Alternativas", questao);

        }

        // GET: Questoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questao questao = await db.Questao.FindAsync(id);
            if (questao == null)
            {
                return HttpNotFound();
            }
            return View(questao);
        }

        // GET: Questoes/Create
        public ActionResult Create(int Idatividade)
        {
            ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa, "Idalternativa", "Descricao");
            ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Descricao");

            Questao questao = new Questao();
            questao.Idatividade = Idatividade;
            return View(questao);
        }

        // POST: Questoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idquestao,Idatividade,IdalternativaCorreta,Descricao")] Questao questao)
        {
            if (ModelState.IsValid)
            {
                db.Questao.Add(questao);
                await db.SaveChangesAsync();
                return VoltarParaListagem(questao);
            }

            ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa, "Idalternativa", "Descricao", questao.IdalternativaCorreta);
            ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Idatividade", questao.Idatividade);
            return View(questao);
        }

        // GET: Questoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questao questao = await db.Questao.FindAsync(id);
            if (questao == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa.Where(a => a.Idquestao == id), "Idalternativa", "Descricao");
            ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Idatividade", questao.Idatividade);
            return View(questao);
        }

        // POST: Questoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idquestao,Idatividade,IdalternativaCorreta,Descricao")] Questao questao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return VoltarParaListagem(questao);
            }
            ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa, "Idalternativa", "Descricao", questao.IdalternativaCorreta);
            ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Idatividade", questao.Idatividade);
            return View(questao);
        }

        // GET: Questoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questao questao = await db.Questao.FindAsync(id);
            if (questao == null)
            {
                return HttpNotFound();
            }
            return View(questao);
        }

        // POST: Questoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Questao questao = await db.Questao.FindAsync(id);
            db.Questao.Remove(questao);
            await db.SaveChangesAsync();
            return VoltarParaListagem(questao);
        }

        //Retorna para a tela principal do Curso
        private RedirectToRouteResult VoltarParaListagem(Questao questao)
        {
            try
            {
                Atividade atividade = db.Atividade.Find(questao.Idatividade);
                return RedirectToAction("Details", "Atividades", new { id = atividade.Idatividade, Idquestao = questao.Idquestao });
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
