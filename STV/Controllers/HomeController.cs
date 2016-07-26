using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using STV.DAL;
using STV.Auth;

namespace STV.Controllers
{
    public class HomeController : Controller
    {

        SessionContext context = new SessionContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View();
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


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario u, string returnUrl = "")
        {
            if (ModelState.IsValid) 
            {
                using (STVDbContext db = new STVDbContext())
                {
                    var usuarioAutenticado = db.Usuario
                        .Where(a => a.Cpf.Equals(u.Cpf) && a.Senha.Equals(u.Senha))
                        .Select(a => new 
                        {
                            Idusuario = a.Idusuario,
                            Nome = a.Nome,
                            Cpf = a.Cpf,
                            Iddepartamento = a.Iddepartamento,
                            Senha = a.Senha
                        }).FirstOrDefault();

                    if (usuarioAutenticado != null)
                    {
                        //FormsAuthentication.SetAuthCookie(usuarioAutenticado.Nome, false);
                        //Session["UsuarioLogadoID"] = usuarioAutenticado.Idusuario.ToString();
                        //Session["UsuarioLogadoNome"] = usuarioAutenticado.Nome.ToString();

                        Usuario UsuarioLogado = new Usuario
                        {
                            Idusuario = usuarioAutenticado.Idusuario,
                            Nome = usuarioAutenticado.Nome,
                            Iddepartamento = usuarioAutenticado.Iddepartamento,
                            Senha = usuarioAutenticado.Senha
                        };

                        context.SetAuthenticationToken(UsuarioLogado.Nome.ToString(), false, UsuarioLogado);

                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index");
                        else
                            return Redirect(returnUrl);
                    }
                    else
                        ViewBag.Message = "CPF e/ou senha incorreto(s)";
                }
            }
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}