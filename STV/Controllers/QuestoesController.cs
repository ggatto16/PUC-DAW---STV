using STV.Auth;
using STV.DAL;
using STV.Models;
using STV.Models.Validation;
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
    public class QuestoesController : Controller
    {
        private STVDbContext db = new STVDbContext();
        private Usuario UsuarioLogado;

        public QuestoesController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }

        // GET: Questoes
        //public async Task<ActionResult> Index()
        //{
        //    var questao = db.Questao.Include(q => q.AlternativaCorreta).Include(q => q.Atividade);
        //    return View(await questao.ToListAsync());
        //}

        // GET: Conteúdo da Unidade
        public async Task<ActionResult> CarregarAlternativas(int? Idquestao)
        {
            try
            {
                if (Idquestao == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                var questao = await db.Questao.FindAsync(Idquestao);

                if (!QuestaoValidation.CanSee(questao, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                var alternativas = from a in db.Alternativa where a.Idquestao == Idquestao select a;
                questao.Alternativas = await alternativas.ToListAsync();

                return PartialView("Alternativas", questao);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Questoes/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Questao questao = await db.Questao.FindAsync(id);

                if (!QuestaoValidation.CanSee(questao, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                return View(questao);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Questoes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? Idatividade)
        {
            if (Idatividade == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Questao questao = new Questao();
            try
            {
                questao.Atividade = db.Atividade.Find(Idatividade);
                AtividadeValidation.CanEdit(questao.Atividade);

                ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa, "Idalternativa", "Descricao");
                ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Descricao");
                questao.Idatividade = (int)Idatividade;

                return View(questao);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(questao);
            }
        }

        // POST: Questoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Idquestao,Idatividade,IdalternativaCorreta,Descricao,Numero")] Questao questao)
        {
            if (ModelState.IsValid)
            {
                db.Questao.Add(questao);
                await db.SaveChangesAsync();
                TempData["msg"] = "Dados salvos!";
                return VoltarParaListagem(questao);
            }

            ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa, "Idalternativa", "Descricao", questao.IdalternativaCorreta);
            ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Idatividade", questao.Idatividade);
            return View(questao);
        }

        // GET: Questoes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            Questao questao = null;
            try
            {
                if (id == null)
                    throw new Exception("Ops! Requisição inválida.");

                questao = await db.Questao.FindAsync(id);
                if (questao == null)
                    throw new Exception("Questão não encontrada.");

                AtividadeValidation.CanEdit(questao.Atividade);

                ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa.Where(a => a.Idquestao == id), "Idalternativa", "Descricao");
                ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Idatividade", questao.Idatividade);
                return View(questao);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(questao);
            }
            catch (Exception ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Idquestao,Idatividade,IdalternativaCorreta,Descricao,Numero")] Questao questao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Dados salvos!";
                return VoltarParaListagem(questao);
            }
            ViewBag.IdalternativaCorreta = new SelectList(db.Alternativa, "Idalternativa", "Descricao", questao.IdalternativaCorreta);
            ViewBag.Idatividade = new SelectList(db.Atividade, "Idatividade", "Idatividade", questao.Idatividade);
            return View(questao);
        }

        // GET: Questoes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            Questao questao = null;
            try
            {
                if (id == null)
                    throw new Exception("Ops! Requisição inválida.");

                questao = await db.Questao.FindAsync(id);
                if (questao == null)
                    throw new Exception("Questão não econtrada.");

                AtividadeValidation.CanEdit(questao.Atividade);

                return View(questao);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(questao);
            }
            catch (Exception ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Questao questao = await db.Questao.FindAsync(id);
            try
            {
                db.Entry(questao).Collection("Alternativas").Load(); //Para remover também a referência
                db.Questao.Remove(questao);
                await db.SaveChangesAsync();
                TempData["msg"] = "Questão excluída!";
                return VoltarParaListagem(questao);
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Questão não pode ser excluída.";
                return RedirectToAction("Details", "Atividades", new
                {
                    id = questao.Idatividade,
                    Idquestao = questao.Idquestao
                });
            }
        }

        //Retorna para a tela principal do Curso
        private RedirectToRouteResult VoltarParaListagem(Questao questao)
        {
            Atividade atividade = db.Atividade.Find(questao.Idatividade);
            return RedirectToAction("Details", "Atividades", new { id = atividade.Idatividade, Idquestao = questao.Idquestao });
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
