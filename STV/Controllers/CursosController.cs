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
using STV.ViewModels;
using AutoMapper;
using STV.DAL;
using System.Web.Security;
using STV.Auth;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;

namespace STV.Controllers
{
    [Authorize(Roles = "Admin, Default")]
    public class CursosController : Controller
    {
        private STVDbContext db = new STVDbContext();
        private Usuario UsuarioLogado;

        public CursosController()
        {
            SessionContext auth = new SessionContext();
            //UsuarioLogado = Mapper.Map<DadosUsuario, Usuario>(auth.GetUserData());
            UsuarioLogado = auth.GetUserData();
        }

        [Authorize]
        public async Task<ActionResult> MeusCursos()
        {
            int Idusuario = UsuarioLogado.Idusuario;
            var cursos = db.Curso.Where(x => x.Usuarios.Any(d => d.Idusuario == Idusuario));

            ViewBag.Idusuario = Idusuario;
            return View(await cursos.ToListAsync());
        }


        public async Task<ActionResult> MeusCursosGerenciaveis()
        {
            int Idusuario = UsuarioLogado.Idusuario;
            var cursos = db.Curso.Where(x => x.Instrutor.Idusuario == Idusuario);

            ViewBag.Idusuario = Idusuario;
            ViewBag.Gerenciar = true;
            return View("CursosDisponiveis", await cursos.ToListAsync());
        }

        [Authorize]
        public async Task<ActionResult> CursosDisponiveis()
        {
            int Idusuario = UsuarioLogado.Idusuario;

            //int Iddepartamento = db.Usuario.Where(u => u.Idusuario == Idusuario).Select(u => u.Iddepartamento).FirstOrDefault();
            int Iddepartamento = UsuarioLogado.Iddepartamento;

            //Lista os cursos disponíveis de acordo com o departamento do usuário, exceto os cursos cujo instrutor é o próprio usuário
            var cursos = db.Curso.Where(x => x.Departamentos
                            .Any(d => d.Iddepartamento == Iddepartamento) && x.IdusuarioInstrutor != Idusuario)
                                .Include(u => u.Usuarios);

            ViewBag.Idusuario = Idusuario;

            return View(await cursos.ToListAsync());
        }

        public async Task<ActionResult> Inscrever(int Idcurso)
        {
            int Idusuario = UsuarioLogado.Idusuario;
            var curso = await db.Curso.FindAsync(Idcurso);
            Usuario usuario = await db.Usuario.FindAsync(Idusuario);

            curso.Usuarios.Add(usuario);

            await db.SaveChangesAsync();

            return RedirectToAction("CursosDisponiveis");
        }

        private void CarregarDepartamentos(Curso curso)
        {
            var allDepartamentos = db.Departamento;
            var cursoDepartamentos = new HashSet<int>(curso.Departamentos.Select(c => c.Iddepartamento));
            var viewModel = new List<DepartamentosAtribuidos>();
            foreach (var departamento in allDepartamentos)
            {
                viewModel.Add(new DepartamentosAtribuidos
                {
                    Iddepartamento = departamento.Iddepartamento,
                    Descricao = departamento.Descricao,
                    Atribuido = cursoDepartamentos.Contains(departamento.Iddepartamento)
                });
            }
            ViewBag.Departamentos = viewModel;
        }


        // GET: Cursos
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Curso.ToListAsync());
        }

        // GET: Cursos/Details/5
        public async Task<ActionResult> Details(int? id, int? Idunidade)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Curso curso = await db.Curso.FindAsync(id);

            Curso curso = await db.Curso
                .Include(c => c.Instrutor)
                .Include(c => c.Unidades)
                .Where(c => c.Idcurso == id)
                .SingleAsync();


            if (curso == null)
                return HttpNotFound();

            curso.NotaCursoAtual = await db.NotaCurso.FindAsync(UsuarioLogado.Idusuario, curso.Idcurso);

            ViewBag.UnidadeSelecionada = Idunidade;  //para reabrir o conteúdo

            //Verificar se é instrutor
            var cursoVerify = await db.Curso
                .Where(c => c.IdusuarioInstrutor == UsuarioLogado.Idusuario && c.Idcurso == id)
                .SingleOrDefaultAsync();
            ViewBag.Gerenciar = cursoVerify != null ? true : false;

            //mesclando model curso com viewmodel cursoVM
            //var cursoVM = Mapper.Map<Curso, cursoVM>(curso);

            ////listando as unidades do curso e complementando a viewmodel com esses dados
            //var unidadesdocurso = from u in db.Unidade where u.Curso.Idcurso == curso.Idcurso select u;
            //if (unidadesdocurso.Count() > 0) cursoVM.Unidades = new List<Unidade>();
            //foreach (var unidade in unidadesdocurso)
            //{
            //    cursoVM.Unidades.Add(unidade);
            //    var atividadesdaunidade = from a in db.Atividade where a.Idunidade == unidade.Idunidade select a;
            //    if (atividadesdaunidade.Count() > 0) cursoVM.Atividades = new List<Atividade>();
            //    foreach (var atividade in atividadesdaunidade)
            //    {
            //        cursoVM.Atividades.Add(atividade);
            //    }
            //}

            return View(curso);
        }

        // GET: Cursos/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var curso = new Curso();
            curso.Departamentos = new List<Departamento>();
            CarregarDepartamentos(curso);
            ViewBag.IdusuarioInstrutor = new SelectList(db.Usuario, "Idusuario", "Nome");
            return View();
        }

        // POST: Cursos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idcurso,Titulo,Dtinicial,Dtfinal,IdusuarioInstrutor,Categoria,Palavraschave")] Curso curso, string[] departamentosSelecionados)
        {
            if (departamentosSelecionados != null)
            {
                curso.Departamentos = new List<Departamento>();
                foreach (var departamento in departamentosSelecionados)
                {
                    var departamentoToAdd = db.Departamento.Find(int.Parse(departamento));
                    curso.Departamentos.Add(departamentoToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                curso.Instrutor = db.Usuario.Find(curso.IdusuarioInstrutor);
                curso.Stamp = DateTime.Now;
                db.Curso.Add(curso);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(curso);
        }

        [Authorize(Roles = "Admin")]
        // GET: Cursos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Curso curso = await db.Curso.FindAsync(id);

            if (curso == null)
                return HttpNotFound();

            CarregarDepartamentos(curso);

            ViewBag.IdusuarioInstrutor = new SelectList(db.Usuario, "Idusuario", "Nome");
            return View(curso);
        }

        // POST: Cursos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, string[] departamentosSelecionados)
        {
            var cursoToUpdate = await db.Curso
                  .Include(u => u.Departamentos)
                  .Include(u => u.Instrutor)
                  .Where(i => i.Idcurso == id)
                  .SingleAsync();

            if (TryUpdateModel(cursoToUpdate, "",
                   new string[] { "Titulo", "Dtinicial", "Dtfinal", "IdusuarioInstrutor", "Categoria", "Palavraschave" }))
            {
                try
                {
                    AtualizarVisibilidadeDepartamentos(departamentosSelecionados, cursoToUpdate);
                    cursoToUpdate.Stamp = DateTime.Now;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            CarregarDepartamentos(cursoToUpdate);
            ViewBag.IdusuarioInstrutor = new SelectList(db.Usuario, "Idusuario", "Nome");
            return View(cursoToUpdate);
        }

        [Authorize(Roles = "Admin")]
        private void AtualizarVisibilidadeDepartamentos(string[] departamentoSelecionados, Curso cursoToUpdate)
        {
            if (departamentoSelecionados == null)
            {
                cursoToUpdate.Departamentos = new List<Departamento>();
                return;
            }

            var departamentoSelecionadosHS = new HashSet<string>(departamentoSelecionados);

            var cursoDepartamentos = new HashSet<int>
                (cursoToUpdate.Departamentos.Select(c => c.Iddepartamento));

            foreach (var departamento in db.Departamento)
            {
                if (departamentoSelecionadosHS.Contains(departamento.Iddepartamento.ToString()))
                {
                    if (!cursoDepartamentos.Contains(departamento.Iddepartamento))
                    {
                        cursoToUpdate.Departamentos.Add(departamento);
                    }
                }
                else
                {
                    if (cursoDepartamentos.Contains(departamento.Iddepartamento))
                    {
                        cursoToUpdate.Departamentos.Remove(departamento);
                    }
                }
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Cursos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = await db.Curso.FindAsync(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Cursos/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Curso curso = await db.Curso.FindAsync(id);
            db.Entry(curso).Collection("Departamentos").Load();
            db.Curso.Remove(curso);
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
