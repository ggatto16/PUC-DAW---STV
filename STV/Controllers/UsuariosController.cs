using AutoMapper;
using iTextSharp.text;
using MvcRazorToPdf;
using STV.Auth;
using STV.DAL;
using STV.Models;
using STV.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace STV.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private STVDbContext db = new STVDbContext();
        private string admin = ConfigurationManager.AppSettings["AdmUserId"].ToString();
        private Usuario UsuarioLogado;

        public UsuariosController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Relatorio(int id)
        {
            Usuario usuario = db.Usuario.Find(id);

            var RelatorioUsuario = Mapper.Map<Usuario, RelatorioUsuario>(usuario);

            // return new PdfActionResult("PDF", RelatorioUsuario);
            //return View("Relatorio", RelatorioUsuario);

            return new PdfActionResult("Relatorio", RelatorioUsuario, (writer, document) =>
            {
                document.SetPageSize(PageSize.A4);
                document.NewPage();
                document.AddCreator("teste");
                HttpContext.Response.AddHeader("content-disposition", string.Format("inline; filename=Relatorio-{0}.pdf", usuario.Nome));
            });
        }

        // GET: Usuarios
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string cpf, string nome)
        {
            ViewBag.FiltroCPF = cpf;
            ViewBag.FiltroNome = nome;
            ViewBag.MensagemSucesso = TempData["msg"];
            ViewBag.MensagemErro = TempData["msgErr"];
            TempData.Clear();

            IQueryable<Usuario> usuarios;

            if (!string.IsNullOrEmpty(cpf))
                usuarios = db.Usuario.Where(u => u.Cpf == cpf && u.Cpf != admin);
            else if (!string.IsNullOrEmpty(nome))
                usuarios = db.Usuario.Where(u => u.Nome.Contains(nome) && u.Cpf != admin);
            else
                usuarios = db.Usuario.Where(u => u.Cpf != admin);

            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Idusuario,Cpf,Nome,Email,SenhaDigitada,SenhaDigitadaConfirmacao,Iddepartamento")] UsuarioVM usuarioVM, string[] rolesSelecionadas)
        {

            if (usuarioVM.Iddepartamento == 0)
                ModelState.AddModelError("", "Departamento é obrigatório.");

            if (db.Usuario.Any(u => u.Cpf == usuarioVM.CpfSoNumeros))
                ModelState.AddModelError("", "Já existe um usuário com este CPF cadastrado no sistema.");

            if (rolesSelecionadas != null && rolesSelecionadas.Count() > 0)
            {
                usuarioVM.Roles = new List<Role>();
                foreach (var role in rolesSelecionadas)
                {
                    var roleToAdd = db.Role.Find(int.Parse(role));
                    usuarioVM.Roles.Add(roleToAdd);
                }
            }
            else
                ModelState.AddModelError("", "Selecione uma role para atribuir ao usuário.");

            var usuario = Mapper.Map<UsuarioVM, Usuario>(usuarioVM);
            usuario.Stamp = DateTime.Now;
            usuario.Senha = Crypt.Encrypt(ConfigurationManager.AppSettings["DefaultPassword"]);
            usuario.Cpf = usuarioVM.CpfSoNumeros;

            if (ModelState.IsValid)
            {
                db.Usuario.Add(usuario);
                await db.SaveChangesAsync();
                TempData["msg"] = "Usuário criado!";
                return RedirectToAction("Index");
            }

            usuarioVM.Roles = usuario.Roles;
            CarregarRolesDisponiveis(usuarioVM);
            ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao");
            return View(usuarioVM);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "Admin")]
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

                var usuarioVM = Mapper.Map<Usuario, UsuarioEditVM>(usuario);

                CarregarRolesDisponiveis(usuarioVM);
                ViewBag.Iddepartamento = new SelectList(db.Departamento, "Iddepartamento", "Descricao", usuario.Iddepartamento);

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
        [Authorize(Roles = "Admin")]
        //public async Task<ActionResult> Edit([Bind(Include = "Idusuario,Cpf,Nome,Email,Senha,Iddepartamento,Role")] Usuario usuario)
        public async Task<ActionResult> Edit([Bind(Include = "Idusuario,Cpf,Nome,Email,Iddepartamento")] UsuarioEditVM usuarioVM, string[] rolesSelecionadas)
        {
            var usuarioToUpdate = Mapper.Map<UsuarioEditVM, Usuario>(usuarioVM);

            //Preenche o modelo com a senha
            var usu = db.Usuario
                    .Where(u => u.Idusuario == usuarioToUpdate.Idusuario)
                    .Select(u => new { Senha = u.Senha }).ToList();
            usuarioToUpdate.Senha = usu.FirstOrDefault().Senha;

            //Faz as validações necessárias
            if (usuarioToUpdate.Iddepartamento == null)
                ModelState.AddModelError("", "Departamento é obrigatório.");

            if (db.Usuario.Any(u => u.Cpf == usuarioVM.CpfSoNumeros && u.Idusuario != usuarioVM.Idusuario))
                ModelState.AddModelError("", "Já existe um usuário com este CPF cadastrado no sistema.");

            if (rolesSelecionadas == null)
                ModelState.AddModelError("", "Selecione uma role para atribuir ao usuário.");

            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Atualiza os dados
                        usuarioToUpdate.Cpf = usuarioVM.CpfSoNumeros;
                        usuarioToUpdate.Stamp = DateTime.Now;
                        db.Entry(usuarioToUpdate).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        //Atualiza as roles
                        var user = db.Usuario.Include("Roles")
                                    .Single(u => u.Idusuario == usuarioToUpdate.Idusuario);
                        foreach (var role in user.Roles.ToList())
                        {
                            if (!rolesSelecionadas.Contains(role.Idrole.ToString()))
                                user.Roles.Remove(role);
                        }
                        foreach (var newRoleId in rolesSelecionadas)
                        {
                            if (!user.Roles.Any(r => r.Idrole.ToString() == newRoleId))
                            {
                                var newRole = new Role { Idrole = Convert.ToInt32(newRoleId) };
                                db.Role.Attach(newRole);
                                user.Roles.Add(newRole);
                            }
                        }
                        await db.SaveChangesAsync();

                        transaction.Commit();
                        TempData["msg"] = "Dados Salvos!";
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            usuarioVM.Roles = usuarioToUpdate.Roles;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        private void CarregarRolesDisponiveis(UsuarioEditVM usuario)
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

        public async Task<ActionResult> ResetPassword(int id)
        {
            var usuario = db.Usuario.Find(id);

            if (usuario == null)
            {
                TempData["msgErr"] = "Usuário não encontrado";
                return RedirectToAction("Index");
            }

            usuario.Senha = Crypt.Encrypt(ConfigurationManager.AppSettings["DefaultPassword"]);
            db.Entry(usuario).State = EntityState.Modified;
            await db.SaveChangesAsync();
            TempData["msg"] =string.Format("A senha de {0} foi redefinida para a senha padrão ({1})", usuario.Nome, ConfigurationManager.AppSettings["DefaultPassword"]);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AlterarSenha()
        {
            try
            {

                string usuario = db.Usuario
                    .Where(u => u.Idusuario == UsuarioLogado.Idusuario)
                    .Select(u => u.Nome).Single();

                if (usuario == null)
                    throw new ApplicationException("Usuário não encontrado.");

                AlterarSenhaVM vm = new AlterarSenhaVM();
                vm.Nome = usuario;
                vm.Idusuario = UsuarioLogado.Idusuario;
                return View(vm);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Default")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AlterarSenha([Bind(Include = "Idusuario,Senha,SenhaDigitada,SenhaDigitadaConfirmacao")] AlterarSenhaVM usuarioVM)
        {

            var usuario = db.Usuario.Find(usuarioVM.Idusuario);

            if (!(Crypt.Decrypt(usuario.Senha) == usuarioVM.Senha))
                ModelState.AddModelError("", "Senha atual inválida.");

            if (!usuarioVM.SenhaDigitada.Equals(usuarioVM.SenhaDigitadaConfirmacao))
                ModelState.AddModelError("", "Senha e Confirmação de Senha não correspondem.");

            if (ModelState.IsValid)
            {
                usuario.Senha = Crypt.Encrypt(usuarioVM.SenhaDigitada);
                db.Usuario.Attach(usuario);
                db.Entry(usuario).Property(u => u.Senha).IsModified = true;
                await db.SaveChangesAsync();
                TempData["msg"] = "Senha alterada!";
                return RedirectToAction("Index", "Home");
            }

            return View(usuarioVM);
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
