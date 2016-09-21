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
using System.Data.Entity.Infrastructure;
using MvcRazorToPdf;
using iTextSharp.text;
using STV.Auth;

namespace STV.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private STVDbContext db = new STVDbContext();

        public ActionResult PDF(int id)
        {
            Usuario usuario = db.Usuario.Find(id);

            var RelatorioUsuario = Mapper.Map<Usuario, RelatorioUsuario>(usuario);

            return new PdfActionResult("PDF", RelatorioUsuario);
            //return View("PDF", RelatorioUsuario);

            //return new PdfActionResult(usuario, (writer, document) =>
            //{
            //    document.SetPageSize(PageSize.A4.Rotate());
            //    document.NewPage();

            //});
        }

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
            var usuario = new Usuario();
            usuario.Roles = new List<Role>();
            CarregarRolesDisponiveis(usuario);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idusuario,Cpf,Nome,Email,Senha,Iddepartamento")] Usuario usuario, string[] rolesSelecionadas)
        {
            if (rolesSelecionadas != null)
            {
                usuario.Roles = new List<Role>();
                foreach (var role in rolesSelecionadas)
                {
                    var roleToAdd = db.Role.Find(int.Parse(role));
                    usuario.Roles.Add(roleToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                usuario.Stamp = DateTime.Now;
                usuario.Status = false;
                usuario.Senha = Crypt.Encrypt(usuario.Senha);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuario usuario = await db.Usuario
                .Include(u => u.Departamento)
                .Include(r => r.Roles)
                .Where(u => u.Idusuario == id)
                .SingleAsync();
                
            if (usuario == null)
                return HttpNotFound();

            CarregarRolesDisponiveis(usuario);
            //CarregarDepartamentos(usuario.Iddepartamento);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao", usuario.Iddepartamento);

            var usuarioVM = Mapper.Map<Usuario, UsuarioVM>(usuario);
            usuarioVM.SenhaDigitada = Crypt.Decrypt(usuario.Senha);

            return View(usuarioVM);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Idusuario,Cpf,Nome,Email,Senha,Iddepartamento,Role")] Usuario usuario)
        public async Task<ActionResult> Edit(int? id, string[] rolesSelecionadas, string SenhaDigitada)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var usuarioToUpdate = await db.Usuario
                  .Include(u => u.Departamento)
                  .Include(u => u.Roles)
                  .Where(i => i.Idusuario == id)
                  .SingleAsync();

            if (TryUpdateModel(usuarioToUpdate, "",
                   new string[] { "Cpf", "Nome", "Email", "Iddepartamento" }))
            {
                try
                {
                    AtualizarRolesUsuario(rolesSelecionadas, usuarioToUpdate);
                    usuarioToUpdate.Stamp = DateTime.Now;
                    usuarioToUpdate.Senha = Crypt.Encrypt(SenhaDigitada);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            CarregarRolesDisponiveis(usuarioToUpdate);
            //CarregarDepartamentos(usuarioToUpdate.Iddepartamento);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao", usuarioToUpdate.Iddepartamento);
            return View(usuarioToUpdate);
        }

        private void AtualizarRolesUsuario(string[] rolesSelecionadas, Usuario usuarioToUpdate)
        {
            if (rolesSelecionadas == null)
            {
                usuarioToUpdate.Roles = new List<Role>();
                return;
            }

            var rolesSelecionadasHS = new HashSet<string>(rolesSelecionadas);

            var instructorCourses = new HashSet<int>
                (usuarioToUpdate.Roles.Select(c => c.Idrole));

            foreach (var role in db.Role)
            {
                if (rolesSelecionadasHS.Contains(role.Idrole.ToString()))
                {
                    if (!instructorCourses.Contains(role.Idrole))
                    {
                        usuarioToUpdate.Roles.Add(role);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(role.Idrole))
                    {
                        usuarioToUpdate.Roles.Remove(role);
                    }
                }
            }
        }

        private void CarregarDepartamentos(object departamentoSelecionado = null)
        {
            var departmentosQuery = from d in db.Departamento
                                   orderby d.Descricao
                                   select d;
            ViewBag.Iddepartamento = new SelectList(departmentosQuery, "Iddepartamento", "Descricao", departamentoSelecionado);
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
            db.Entry(usuario).Collection("Roles").Load();
            db.Usuario.Remove(usuario);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void CarregarRolesDisponiveis(Usuario usuario)
        {
            var allRoles = db.Role;
            var usuarioRoles = new HashSet<int>(usuario.Roles.Select(c => c.Idrole));
            var viewModel = new List<RolesAtribuidas>();
            foreach (var role in allRoles)
            {
                viewModel.Add(new RolesAtribuidas
                {
                    Idrole = role.Idrole,
                    Nome = role.Nome,
                    Atribuida = usuarioRoles.Contains(role.Idrole)
                });
            }
            ViewBag.Roles = viewModel;
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
