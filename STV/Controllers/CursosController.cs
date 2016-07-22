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

namespace STV.Controllers
{
    public class CursosController : Controller
    {
        private STVDbContext db = new STVDbContext();

        // GET: Cursos
        public async Task<ActionResult> Index()
        {
            return View(await db.Curso.ToListAsync());
        }

        // GET: Cursos/Details/5
        public async Task<ActionResult> Details(int? id, int? Idunidade)
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

            ViewBag.UnidadeSelecionada = Idunidade;  //para reabrir o conteúdo

            //mesclando model curso com viewmodel cursoVM
            var cursoVM = Mapper.Map<Curso, cursoVM>(curso);

            //listando as unidades do curso e complementando a viewmodel com esses dados
            var unidadesdocurso = from u in db.Unidade where u.Curso.Idcurso == curso.Idcurso select u;
            if (unidadesdocurso.Count() > 0) cursoVM.Unidades = new List<Unidade>();
            foreach (var unidade in unidadesdocurso)
            {
                cursoVM.Unidades.Add(unidade);
                var atividadesdaunidade = from a in db.Atividade  where a.Idunidade == unidade.Idunidade select a;
                if (atividadesdaunidade.Count() > 0) cursoVM.Atividades = new List<Atividade>();
                foreach (var atividade in atividadesdaunidade)
                {
                    cursoVM.Atividades.Add(atividade);
                }
            }
            //ViewBag.Unidades = viewModel;

            return View(cursoVM);
        }

        // GET: Cursos/Create
        public ActionResult Create()
        {
            ViewBag.Idusuario = new SelectList(db.Usuario, "Idusuario", "Nome");
            return View();
        }

        // POST: Cursos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idcurso,Titulo,Dtinicial,Dtfinal,Idusuario,Categoria,Palavraschave,Stamp")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                curso.Stamp = DateTime.Now;
                db.Curso.Add(curso);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(curso);
        }

        // GET: Cursos/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.Idusuario = new SelectList(db.Usuario, "Idusuario", "Nome");
            return View(curso);
        }

        // POST: Cursos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idcurso,Titulo,Dtinicial,Dtfinal,Idusuario,Categoria,Palavraschave,Stamp")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                curso.Stamp = DateTime.Now;
                db.Entry(curso).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(curso);
        }

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Curso curso = await db.Curso.FindAsync(id);
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
