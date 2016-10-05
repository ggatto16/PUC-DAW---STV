using AutoMapper;
using iTextSharp.text;
using MvcRazorToPdf;
using STV.Auth;
using STV.DAL;
using STV.Models;
using STV.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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

            // return new PdfActionResult("PDF", RelatorioUsuario);
            //return View("PDF", RelatorioUsuario);

            return new PdfActionResult("PDF", RelatorioUsuario, (writer, document) =>
            {
                document.SetPageSize(PageSize.A4);
                document.NewPage();
                document.AddCreator("teste");
                HttpContext.Response.AddHeader("content-disposition", string.Format("inline; filename=Relatorio-{0}.pdf", usuario.Nome));
            });
        }

        // GET: Usuarios
        public async Task<ActionResult> Index(string cpf, string nome)
        {
            ViewBag.FiltroCPF = cpf;
            ViewBag.FiltroNome = nome;
            ViewBag.MensagemSucesso = TempData["msg"];
            ViewBag.MensagemErro = TempData["msgErr"];
            TempData.Clear();

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
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");
                
                Usuario usuario = await db.Usuario.FindAsync(id);
                if (usuario == null)
                    throw new ApplicationException("Usuário não encontrado.");

                var usuarioVM = Mapper.Map<Usuario, UsuarioVM>(usuario);
                
                return View(usuarioVM);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            var usuario = new UsuarioVM();
            usuario.Roles = new List<Role>();
            CarregarRolesDisponiveis(usuario);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao");

            return View(usuario);
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idusuario,Cpf,Nome,Email,Senha,Iddepartamento")] UsuarioVM usuarioVM, string[] rolesSelecionadas)
        {
            var usuario = Mapper.Map<UsuarioVM, Usuario>(usuarioVM);

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
                TempData["msg"] = "Usuário criado!";
                return RedirectToAction("Index");
            }

            usuarioVM.Roles = new List<Role>();
            CarregarRolesDisponiveis(usuarioVM);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao");
            return View(usuarioVM);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Usuario usuario = db.Usuario
                    .Include(u => u.Departamento)
                    .Include(r => r.Roles)
                    .Where(u => u.Idusuario == id)
                    .Single();

                if (usuario == null)
                    throw new ApplicationException("Usuário não encontrado.");

                var usuarioVM = Mapper.Map<Usuario, UsuarioVM>(usuario);

                CarregarRolesDisponiveis(usuarioVM);
                ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao", usuario.Iddepartamento);
                usuarioVM.SenhaDigitada = Crypt.Decrypt(usuario.Senha);

                return View(usuarioVM);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index");
            }
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
            {
                TempData["msgErr"] = "Ops! Requisição inválida.";
                return RedirectToAction("Index");
            }

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
                    TempData["msg"] = "Dados Salvos!";
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Não foi possível salvar as alterações.");
                }
            }

            var usuarioVM = Mapper.Map<Usuario, UsuarioVM>(usuarioToUpdate);
            CarregarRolesDisponiveis(usuarioVM);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao", usuarioVM.Iddepartamento);
            return View(usuarioVM);
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
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");
                
                Usuario usuario = await db.Usuario.FindAsync(id);
                if (usuario == null)
                    throw new ApplicationException("Usuário não encontrado.");

                return View(usuario);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Usuario usuario = await db.Usuario.FindAsync(id);
                db.Entry(usuario).Collection("Roles").Load();
                db.Usuario.Remove(usuario);
                await db.SaveChangesAsync();
                TempData["msg"] = "Usuário excluído!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Usuário não pode ser excluído.";
                return RedirectToAction("Index");
            }
        }

        private void CarregarRolesDisponiveis(UsuarioVM usuario)
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
