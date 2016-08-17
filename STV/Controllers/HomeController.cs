﻿using STV.Models;
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
    [Authorize]
    public class HomeController : Controller
    {

        SessionContext context = new SessionContext();

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
                            Roles = a.Roles,
                            Medalhas = a.Medalhas
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
                            Medalhas = usuarioAutenticado.Medalhas
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
            return View(Anonymous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private Medalha VerificarMedalhas()
        {
            return new Medalha();
        }

    }
}