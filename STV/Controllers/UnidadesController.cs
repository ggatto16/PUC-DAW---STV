using AutoMapper;
using STV.Auth;
using STV.DAL;
using STV.Models;
using STV.Models.Validation;
using STV.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STV.Controllers
{
    [Authorize]
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
            ViewBag.MensagemSucesso = TempData["msg"];
            ViewBag.MensagemErro = TempData["msgErr"];
            TempData.Clear();

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
            try
            {
                if (idunidade == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                var unidade = await db.Unidade
                    //.Include(u => u.Atividades)
                    //.Include(u => u.Materiais)
                    .Where(u => u.Idunidade == idunidade)
                    .SingleOrDefaultAsync();

                if (!UnidadeValidation.CanSee(unidade, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                var unidadeVM = Mapper.Map<Unidade, UnidadeVM>(unidade);

                IEnumerable<Atividade> atividades;
                if (User.IsInRole("Admin"))
                {
                    atividades = db.Atividade
                        .Where(a => a.Idunidade == unidade.Idunidade).ToList();
                }
                else
                {
                    atividades = db.Atividade
                        .Where(a => a.Idunidade == unidade.Idunidade && a.Questoes
                            .Where(q => q.Alternativas.Count() > 0).Count() > 0).ToList();
                }

                var atividadesVM = Mapper.Map<IEnumerable<Atividade>, IEnumerable<AtividadeVM2>>(atividades);

                unidadeVM.AtividadesVM = atividadesVM;

                foreach (var atividade in unidadeVM.AtividadesVM)
                {
                    int respondidas = await db.Resposta
                        .Where(r => r.Idusuario == UsuarioLogado.Idusuario && r.Questao.Idatividade == atividade.Idatividade)
                        .CountAsync();

                    atividade.Realizado += atividade.PorcentagemQuestao * respondidas;

                    int nota = db.Nota.Where(n => n.Idusuario == UsuarioLogado.Idusuario && n.Idatividade == atividade.Idatividade).Count();
                    if (nota > 0)
                        atividade.IsFinalizada = true;
                }

                //Verificar se é instrutor
                var cursoVerify = await db.Curso
                    .Where(c => c.IdusuarioInstrutor == UsuarioLogado.Idusuario && c.Idcurso == unidadeVM.Idcurso)
                    .SingleOrDefaultAsync();

                unidadeVM.IsInstutor = cursoVerify != null ? true : false;

                ViewBag.UnidadeSelecionada = unidadeVM.Idunidade;
                return PartialView("Conteudo", unidadeVM);
            }
            catch(ApplicationException ex)
            {
                TempData["MsgErr"] = ex.Message;
                return RedirectToAction("Index", "Cursos");
            }
        }

        // GET: Unidades/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Unidade unidade = await db.Unidade.FindAsync(id);
                if (unidade == null)
                    throw new ApplicationException("Unidade não encontrada.");

                if (!UnidadeValidation.CanSee(unidade, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                return View(unidade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        private void ValidarDatas(ref Unidade uni)
        {
            List<string> erros = new List<string>();

            //Verificar se não nenhuma atividade com data de encerramento anterior a data de abertura da unidade em questão
            int Idunidade = uni.Idunidade;
            var atividades = db.Atividade.Where(a => a.Unidade.Idunidade == Idunidade);
            foreach (var atv in atividades)
            {
                if(atv.DataEncerramento < uni.DataAbertura)
                    erros.Add(string.Format("Data de abertura não pode ser posterior à data de encerramento de qualquer uma das atividades. Atividade {0} tem data de encerramento para {1}", atv.Descricao, atv.DataEncerramento));
            }

            var dataAberturaCurso = db.Curso.Find(uni.Idcurso).DataInicial;
            if (dataAberturaCurso != null)
            {
                if (uni.DataAbertura < dataAberturaCurso)
                    erros.Add("Data de abertura não pode ser anterior à data de abertura do curso.");
            }

            AddErrors(erros);
        }

        private void AddErrors(List<string> erros)
        {
            foreach (var error in erros)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: Unidades/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? Idcurso)
        {
            if (Idcurso == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Unidade unidade = null;
            try
            {
                Curso curso = db.Curso.Find(Idcurso);

                if (!CursoValidation.CanEdit(curso, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                unidade = new Unidade { Curso = curso };

                ViewBag.Idcurso = new SelectList(db.Curso, "Idcurso", "Titulo");
                return View(unidade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                if (unidade == null) return RedirectToAction("Index", "Cursos");
                return VoltarParaListagem(unidade);
            }
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Idunidade,Idcurso,Titulo,DataAbertura,Encerrada")] Unidade unidade)
        {
            ValidarDatas(ref unidade);

            if (ModelState.IsValid)
            {
                unidade.Stamp = DateTime.Now;
                db.Unidade.Add(unidade);
                await db.SaveChangesAsync();
                TempData["msg"] = "Unidade criada!";
                return VoltarParaListagem(unidade);
            }

            Curso curso = db.Curso.Find(unidade.Idcurso);
            unidade = new Unidade { Curso = curso };
            return View(unidade);
        }

        // GET: Unidades/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                Unidade unidade = await db.Unidade.FindAsync(id);
                if (unidade == null)
                    throw new ApplicationException("Unidade não encontrada.");

                if (!CursoValidation.CanEdit(unidade.Curso, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                ViewBag.Idcurso = new SelectList(db.Curso, "Idcurso", "Titulo");
                return View(unidade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Idunidade,Idcurso,Titulo,DataAbertura,Encerrada")] Unidade unidade)
        {
            ValidarDatas(ref unidade);

            if (ModelState.IsValid)
            {
                unidade.Stamp = DateTime.Now;
                db.Entry(unidade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Dados Salvos!";
                return VoltarParaListagem(unidade);
            }
            return View(unidade);
        }

        // GET: Unidades/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            Unidade unidade = null;
            try
            {
                if (id == null)
                    throw new Exception("Ops! Requisição inválida.");

                unidade = await db.Unidade.FindAsync(id);

                if (unidade == null)
                    throw new Exception("Unidade não encontrada.");

                if (!CursoValidation.CanEdit(unidade.Curso, UsuarioLogado.Idusuario, User))
                    return View("NaoAutorizado");

                return View(unidade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return VoltarParaListagem(unidade);
            }
            catch (Exception ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Unidade unidade = await db.Unidade.FindAsync(id);
            try
            {
                db.Entry(unidade).Collection("Atividades").Load(); //Para remover também a referência
                db.Entry(unidade).Collection("Materiais").Load(); //Para remover também a referência
                db.Unidade.Remove(unidade);
                await db.SaveChangesAsync();
                TempData["msg"] = "Unidade excluída!";
                return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso });
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Unidade não pode ser excluída.";
                return RedirectToAction("Index", "Home");
            }
        }

        //Retorna para a tela principal do Curso
        private RedirectToRouteResult VoltarParaListagem(Unidade unidade)
        {
            if (unidade.Idunidade == 0)
                return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso });
            else
                return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso, Idunidade = unidade.Idunidade });
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
