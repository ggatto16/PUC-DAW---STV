using STV.DAL;
using STV.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STV.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DepartamentosController : Controller
    {
        private STVDbContext db = new STVDbContext();

        // GET: Departamentos
        public async Task<ActionResult> Index(string s)
        {
            ViewBag.Filtro = s;
            ViewBag.MensagemSucesso = TempData["msg"];
            ViewBag.MensagemErro = TempData["msgErr"];

            var departamentos = from d in db.Departamento select d;

            if (!string.IsNullOrEmpty(s))
            {
                departamentos = departamentos.Where(d => d.Descricao.Contains(s));
            }

            return View(await departamentos.ToListAsync());
        }

        // GET: Departamentos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Departamento departamento = await db.Departamento.FindAsync(id);
                if (departamento == null)
                    throw new ApplicationException("Departamento não encontrado.");

                return View(departamento);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Iddepartamento,Descricao,Status,Stamp")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                departamento.Stamp = DateTime.Now;
                db.Departamento.Add(departamento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(departamento);
        }

        // GET: Departamentos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Departamento departamento = await db.Departamento.FindAsync(id);
                if (departamento == null)
                    throw new ApplicationException("Departamento não encontrado.");

                return View(departamento);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Departamentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Iddepartamento,Descricao,Status,Stamp")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                departamento.Stamp = DateTime.Now;
                db.Entry(departamento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(departamento);
        }

        // GET: Departamentos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Departamento departamento = await db.Departamento.FindAsync(id);
                if (departamento == null)
                    throw new ApplicationException("Departamento não encontrado.");

                return View(departamento);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Departamento departamento = await db.Departamento.FindAsync(id);
                db.Departamento.Remove(departamento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Departamento não pode ser excluído.";
                return RedirectToAction("Index");
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
