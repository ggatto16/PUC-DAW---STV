using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using STV.DAL;
using STV.Auth;
using System.Data.Entity;

namespace STV.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private STVDbContext db = new STVDbContext();
        SessionContext auth = new SessionContext();

        private Usuario UsuarioLogado;

        public HomeController()
        {    
            UsuarioLogado = auth.GetUserData();
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {


                return View();

            }
            else
                return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario Anonymous, string returnUrl = "")
        {
            if (ModelState.IsValid) 
            {
                using (STVDbContext db = new STVDbContext())
                {
                    var usuarioAutenticado = db.Usuario
                        .Where(a => a.Cpf.Equals(Anonymous.Cpf) && a.Senha.Equals(Anonymous.Senha))
                        .Select(a => new 
                        {
                            Idusuario = a.Idusuario,
                            Nome = a.Nome,
                            Cpf = a.Cpf,
                            Iddepartamento = a.Iddepartamento,
                            Senha = a.Senha,
                            Roles = a.Roles
                        }).FirstOrDefault();

                    if (usuarioAutenticado != null)
                    {
                        Usuario UsuarioLogado = new Usuario
                        {
                            Idusuario = usuarioAutenticado.Idusuario,
                            Nome = usuarioAutenticado.Nome,
                            Iddepartamento = usuarioAutenticado.Iddepartamento,
                            Senha = usuarioAutenticado.Senha,
                            Roles = usuarioAutenticado.Roles,
                            Medalhas = AtribuirMedalhas(usuarioAutenticado.Idusuario)
                        };

                        auth.SetAuthenticationToken(UsuarioLogado.Nome.ToString(), false, UsuarioLogado);

                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
                        else
                            return Redirect(returnUrl);
                    }
                    else
                        ViewBag.Message = "CPF e/ou senha incorreto(s)";
                }
            }
            return View(Anonymous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private ICollection<Medalha> AtribuirMedalhas(int Idusuario)
        {
            var notas = db.Nota.Include(n => n.Atividade)
            .Where(n => n.Idusuario == Idusuario && n.Atividade.Dtencerramento < DateTime.Now);


            var usuario = db.Usuario.Find(Idusuario);
            List<Medalha> medalhas = db.Medalha.ToList();

            foreach (Medalha m in medalhas)
            {
                bool valeMedalha = false;
                int notaAtividade;

                switch ((Medalhas)m.Idmedalha)
                {
                    //acertou todas as questoões de uma atividade
                    case Medalhas.Sortudo:

                        if (usuario.Medalhas.Contains(m)) break;

                        foreach (var nota in notas)
                        {
                            if (nota.Atividade.Valor != nota.Pontos)
                                continue;
                            else
                            {
                                usuario.Medalhas.Add(m);
                                break;
                            }
                        }
                        break;

                    //acertou todas as questoes de todas as atividade de uma unidade
                    case Medalhas.Nerd:

                        if (usuario.Medalhas.Contains(m)) break;

                        var unidadesUsuario = db.Unidade.Include(u => u.Atividades)
                            .Where(u => u.Curso.Usuarios.Any(c => c.Idusuario == usuario.Idusuario));

                        foreach (var unidade in unidadesUsuario)
                        {
                            foreach (var atv in unidade.Atividades)
                            {
                                notaAtividade = notas.Where(n => n.Atividade.Idatividade == atv.Idatividade
                                    && n.Idusuario == usuario.Idusuario && n.Atividade.Dtencerramento < DateTime.Now)
                                    .Select(n => new { Pontos = n.Pontos }).Single().Pontos;

                                if (atv.Valor != notaAtividade)
                                {
                                    valeMedalha = false;
                                    break;
                                }
                                else
                                    valeMedalha = true;
                            }
                            if (!valeMedalha) break;
                        }
                        if (valeMedalha) usuario.Medalhas.Add(m);
                        break;

                    //acertou todas as questoes de todas as atividade de um curso
                    case Medalhas.Genio:

                        if (usuario.Medalhas.Contains(m)) break;

                        var cursosUsuario = db.Curso.Include(c => c.Unidades)
                            .Where(u => u.Usuarios.Any(x => x.Idusuario == usuario.Idusuario));

                        foreach (var curso in cursosUsuario)
                        {
                            foreach (var uni in curso.Unidades)
                            {
                                foreach (var atv in uni.Atividades)
                                {
                                    notaAtividade = notas.Where(n => n.Atividade.Idatividade == atv.Idatividade
                                        && n.Idusuario == usuario.Idusuario && n.Atividade.Dtencerramento < DateTime.Now)
                                        .Select(n => new { Pontos = n.Pontos }).Single().Pontos;

                                    if (atv.Valor != notaAtividade)
                                    {
                                        valeMedalha = false;
                                        break;
                                    }
                                    else
                                        valeMedalha = true;
                                }
                                if (!valeMedalha) break;
                            }
                            if (!valeMedalha) break;
                        }

                        if (valeMedalha) usuario.Medalhas.Add(m);
                        break;

                    default:
                        break;
                }
            }

            db.Usuario.Attach(usuario);
            db.SaveChanges();
            return usuario.Medalhas;
        }

    }
}