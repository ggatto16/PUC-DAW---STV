using AutoMapper;
using STV.DAL;
using STV.Models;
using STV.Models.Validation;
using STV.ViewModels;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AlternativasController : Controller
    {
        private STVDbContext db = new STVDbContext();

        // GET: Alternativas
        public async Task<ActionResult> Index()
        {
            ViewBag.MensagemSucesso = TempData["msg"];
            ViewBag.MensagemErro = TempData["msgErr"];
            TempData.Clear();

            var alternativa = db.Alternativa.Include(a => a.Questao);
            return View(await alternativa.ToListAsync());
        }

        // GET: Alternativas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Alternativa alternativa = await db.Alternativa.FindAsync(id);
                if (alternativa == null)
                    throw new ApplicationException("Alternativa não encontrada.");

                return View(alternativa);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Alternativas/Create
        public async Task<ActionResult> Create(int Idquestao)
        {
            Alternativa alternativa = new Alternativa();
            try
            {
                ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao");
                alternativa.Idquestao = Idquestao;
                alternativa.Questao = await db.Questao.FindAsync(Idquestao);

                if (alternativa.Questao == null)
                    throw new ApplicationException("Questão não encontrada.");

                AtividadeValidation.CanEdit(alternativa.Questao.Atividade);

                var alternativaVM = Mapper.Map<Alternativa, AlternativaVM>(alternativa);
                if (alternativaVM.Questao.IdalternativaCorreta == null) alternativaVM.IsCorreta = true; //caso ainda não tenha alternativa correta

                return View(alternativaVM);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(alternativa);
            }

        }

        // POST: Alternativas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idalternativa,Idquestao,IsCorreta,Descricao,Justificativa")] AlternativaVM alternativa)
        {
            if (ModelState.IsValid)
            {
                var ModelAlternativa = Mapper.Map<AlternativaVM, Alternativa>(alternativa);

                db.Alternativa.Add(ModelAlternativa);
                await db.SaveChangesAsync();

                if (alternativa.IsCorreta)
                {
                    var questao = await db.Questao.FindAsync(ModelAlternativa.Idquestao);
                    questao.IdalternativaCorreta = ModelAlternativa.Idalternativa;
                    db.Questao.Attach(questao);
                    db.Entry(questao).Property(q => q.IdalternativaCorreta).IsModified = true;
                    await db.SaveChangesAsync();
                    TempData["msg"] = "Dados salvos!";
                }

                return VoltarParaListagemVM(alternativa);
            }

            ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao", alternativa.Idquestao);
            return View(alternativa);
        }

        // GET: Alternativas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Alternativa alternativa = null;
            try
            {
                if (id == null)
                    throw new Exception("Ops! Requisição inválida.");

                alternativa = await db.Alternativa.FindAsync(id);

                if (alternativa == null)
                    throw new Exception("Alternativa não encontrada.");

                AtividadeValidation.CanEdit(alternativa.Questao.Atividade);

                var alternativaVM = Mapper.Map<Alternativa, AlternativaVM>(alternativa);
                ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao", alternativa.Idquestao);
                return View(alternativaVM);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(alternativa);
            }
            catch (Exception ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Alternativas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idalternativa,Idquestao,Descricao, IsCorreta,Justificativa")] AlternativaVM alternativa)
        {
            if (ModelState.IsValid)
            {
                var ModelAlternativa = Mapper.Map<AlternativaVM, Alternativa>(alternativa);

                if (alternativa.IsCorreta)
                {
                    var questao = await db.Questao.FindAsync(ModelAlternativa.Idquestao);
                    questao.IdalternativaCorreta = ModelAlternativa.Idalternativa;
                    db.Entry(questao).Property(q => q.IdalternativaCorreta).IsModified = true;
                }

                db.Entry(ModelAlternativa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Dados salvos!";

                return VoltarParaListagemVM(alternativa);
            }
            ViewBag.Idquestao = new SelectList(db.Questao, "Idquestao", "Descricao", alternativa.Idquestao);
            return View(alternativa);
        }

        // GET: Alternativas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Alternativa alternativa = null;
            try
            {
                if (id == null)
                    throw new Exception("Ops! Requisição inválida.");

                alternativa = await db.Alternativa.FindAsync(id);
                if (alternativa == null)
                    throw new Exception("Alternativa não encontrada.");

                AtividadeValidation.CanEdit(alternativa.Questao.Atividade);

                if (alternativa.Idalternativa == alternativa.Questao.IdalternativaCorreta)
                    throw new ApplicationException("Este alternativa é a correta. Não pode ser excluída.");

                return View(alternativa);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(alternativa);
            }
            catch (Exception ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Alternativas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Alternativa alternativa = await db.Alternativa.FindAsync(id);
                db.Alternativa.Remove(alternativa);
                await db.SaveChangesAsync();
                return VoltarParaListagem(alternativa);
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Alternativa não pode ser excluída.";
                return RedirectToAction("Index", "Home");
            }
        }

        //Retorna para a tela principal da Atividade
        private RedirectToRouteResult VoltarParaListagem(Alternativa alternativa)
        {
            Questao questao = db.Questao.Find(alternativa.Idquestao);
            return RedirectToAction("Details", "Atividades", new { id = questao.Idatividade, Idquestao = alternativa.Idquestao });
        }

        private RedirectToRouteResult VoltarParaListagemVM(AlternativaVM alternativa)
        {
            Questao questao = db.Questao.Find(alternativa.Idquestao);
            return RedirectToAction("Details", "Atividades", new { id = questao.Idatividade, Idquestao = alternativa.Idquestao });
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
