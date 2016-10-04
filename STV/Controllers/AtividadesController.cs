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
using AutoMapper;
using STV.ViewModels;
using STV.Utils;

namespace STV.Controllers
{
    [Authorize]
    public class AtividadesController : Controller
    {
        private STVDbContext db = new STVDbContext();

        private Usuario UsuarioLogado;

        public AtividadesController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }

        public async Task<ActionResult> CarregarAtividade(int? id, int? index)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Atividade atividade = await db.Atividade
                    .Where(a => a.Idatividade == id)
                    .SingleOrDefaultAsync();

                if (atividade == null)
                    throw new ApplicationException("Ops! Atividade não encontrada.");

                if (!Autorizacao.UsuarioInscrito(atividade.Unidade.Curso.Usuarios, UsuarioLogado.Idusuario, User)) return View("NaoAutorizado");

                var AtividadeModel = Mapper.Map<Atividade, AtividadeVM>(atividade);

                if (index == null)
                {
                    AtividadeModel.QuestaoToShow = atividade.Questoes.FirstOrDefault();
                    AtividadeModel.QuestaoToShow.Indice = 0;
                }
                else
                {
                    AtividadeModel.QuestaoToShow = atividade.Questoes.ElementAtOrDefault((int)index + 1);
                    if (AtividadeModel.QuestaoToShow != null)
                        AtividadeModel.QuestaoToShow.Indice = (int)index + 1;
                    else
                    {
                        TempData["msg"] = "Respostas salvas!";
                        return RedirectToAction("Details", "Cursos", new { id = atividade.Unidade.Idcurso, Idunidade = atividade.Idunidade });
                    }
                }

                return View("Atividade", AtividadeModel);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarResposta(AtividadeVM atividade)
        {
            if (ModelState.IsValid)
            {
                Resposta resposta = null;
                resposta = await db.Resposta.FindAsync(UsuarioLogado.Idusuario, atividade.QuestaoToShow.Idquestao);

                if (resposta != null)
                {
                    resposta.Idalternativa = atividade.QuestaoToShow.IdAlternativaSelecionada;
                    db.Resposta.Attach(resposta);
                    db.Entry(resposta).Property(r => r.Idalternativa).IsModified = true;
                }
                else
                {
                    resposta = new Resposta
                    {
                        Idusuario = UsuarioLogado.Idusuario,
                        Idquestao = atividade.QuestaoToShow.Idquestao,
                        Idalternativa = atividade.QuestaoToShow.IdAlternativaSelecionada
                    };
                    db.Resposta.Add(resposta);
                }
                await db.SaveChangesAsync();
            }

            return RedirectToAction("CarregarAtividade", new { Id = atividade.Idatividade, index = atividade.QuestaoToShow.Indice });
        }

        public async Task<ActionResult> Finalizar(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Atividade atividade = await db.Atividade
                    .Where(a => a.Idatividade == id)
                    .SingleOrDefaultAsync();

                if (atividade == null)
                    throw new ApplicationException("Atividade não encontrada.");

                if (!Autorizacao.UsuarioInscrito(atividade.Unidade.Curso.Usuarios, UsuarioLogado.Idusuario, User)) return View("NaoAutorizado");

                var corretas = db.Resposta
                    .Where(r => r.Idusuario == UsuarioLogado.Idusuario && r.Questao.Idatividade == atividade.Idatividade)
                    .OrderBy(r => r.Idquestao)
                    .Select(r => new { Idquestao = r.Idquestao, Idalternativa = r.Idalternativa })
                    .ToList();

                var corretasHS = new HashSet<int>(corretas.Select(r => r.Idalternativa));

                var respondidas = atividade.Questoes
                    .Select(q => new { Idquestao = q.Idquestao, Idalternativa = (int)q.IdalternativaCorreta })
                    .OrderBy(r => r.Idquestao)
                    .ToList();

                var respondidasHS = new HashSet<int>(respondidas.Select(r => r.Idalternativa));

                int total = atividade.Questoes.Count();
                int certas = total - corretasHS.Except(respondidasHS).Count();
                int valorQuestao = atividade.Valor / total;
                int pontos = certas * valorQuestao;

                Nota nota = new Nota
                {
                    Idatividade = atividade.Idatividade,
                    Idusuario = UsuarioLogado.Idusuario,
                    Pontos = pontos
                };

                db.Nota.Add(nota);
                await db.SaveChangesAsync();
                TempData["msg"] = "Atividade Finalizada! Aguarde o encerramento para verificar o gabarito.";

                return RedirectToAction("Details", "Cursos", new { id = atividade.Unidade.Idcurso, Idunidade = atividade.Idunidade });

            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Atividades/Details/5
        public async Task<ActionResult> Details(int? id, int? Idquestao)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Atividade atividade = await db.Atividade.FindAsync(id);

                if (atividade == null)
                    throw new ApplicationException("Atividade não encontrada.");

                if (!Autorizacao.UsuarioInscrito(atividade.Unidade.Curso.Usuarios, UsuarioLogado.Idusuario, User)) return View("NaoAutorizado");

                ViewBag.MensagemSucesso = TempData["msg"];
                ViewBag.MensagemErro = TempData["msgErr"];
                TempData.Clear();
                ViewBag.QuestaoSelecionada = Idquestao;

                return View(atividade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Atividades/Create
        public ActionResult Create(int Idunidade)
        {
            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo");
            Atividade atividade = new Atividade();
            atividade.Idunidade = Idunidade;
            atividade.Unidade = db.Unidade.Find(Idunidade);
            return View(atividade);
        }

        // POST: Atividades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idatividade,Idunidade,Descricao,Valor,Dtabertura,Dtencerramento")] Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                db.Atividade.Add(atividade);
                await db.SaveChangesAsync();
                TempData["msg"] = "Dados salvos!";
                return VoltarParaListagem(atividade);
            }

            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo", atividade.Idunidade);
            return View(atividade);
        }

        // GET: Atividades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Atividade atividade = await db.Atividade.FindAsync(id);
                if (atividade == null)
                    throw new ApplicationException("Atividade não encontrada.");

                ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo", atividade.Idunidade);
                return View(atividade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Atividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idatividade,Idunidade,Descricao,Valor,Dtabertura,Dtencerramento")] Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atividade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["msg"] = "Dados salvos!";
                return VoltarParaListagem(atividade);
            }
            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo", atividade.Idunidade);
            return View(atividade);
        }

        // GET: Atividades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Atividade atividade = await db.Atividade.FindAsync(id);
                if (atividade == null)
                    throw new ApplicationException("Atividade não econtrada.");

                return View(atividade);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Atividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Atividade atividade = await db.Atividade.FindAsync(id);
            try
            {
                db.Atividade.Remove(atividade);
                await db.SaveChangesAsync();
                TempData["msg"] = "Atividade excluída!";
                return VoltarParaListagem(atividade);
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Atividade não pode ser excluída.";
                return RedirectToAction("Details", "Cursos", new { id = atividade.Unidade.Idcurso, Idunidade = atividade.Unidade.Idunidade });
            }
        }

        //Retorna para a tela principal do Curso
        private RedirectToRouteResult VoltarParaListagem(Atividade atividade)
        {
            Unidade unidade = db.Unidade.Find(atividade.Idunidade);
            return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso, Idunidade = atividade.Idunidade });
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
