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
using AutoMapper;
using STV.ViewModels;

namespace STV.Controllers
{
    public class UsuariosController : Controller
    {
        private STVDbContext db = new STVDbContext();

        // GET: Usuarios
        public async Task<ActionResult> Index(string cpf, string nome)
        {
            ViewBag.FiltroCPF = cpf;
            ViewBag.FiltroNome = nome;

            var usuarios = from u in db.Usuario select u;

            if (!string.IsNullOrEmpty(cpf))
            {
                usuarios = usuarios.Where(u => u.Cpf == cpf);
            }
            else if (!string.IsNullOrEmpty(nome))
            {
                usuarios = usuarios.Where(u => u.Nome.Contains(nome));
            }

            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao");
            ViewBag.IdRole = new SelectList(db.Role, "Idrole", "Nome");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idusuario,Cpf,Nome,Email,Senha,Iddepartamento,Role")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Stamp = DateTime.Now;
                usuario.Status = false;
                db.Usuario.Add(usuario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao");

            var usuarioVM = Mapper.Map<Usuario, UsuarioVM>(usuario);

            var roles = from r in db.Role select r;
            usuarioVM.RolesDisponiveis = new List<Role>(db.Role.ToList());

            return View(usuarioVM);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idusuario,Cpf,Nome,Email,Senha,Iddepartamento,Role")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Stamp = DateTime.Now;
                db.Entry(usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Usuario usuario = await db.Usuario.FindAsync(id);
            db.Usuario.Remove(usuario);
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
