using AutoMapper;
using iTextSharp.text;
using MvcRazorToPdf;
using STV.Auth;
using STV.DAL;
using STV.Models;
using STV.Utils;
using STV.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public ActionResult Certificado(int id)
        {
            Curso curso = db.Curso.Find(id);
            //return new PdfActionResult("Certificado", curso);
            //return View("Certificado", curso);

            StringBuilder HTMLString = new StringBuilder();
            HTMLString.Append("    <div class='cert'>");
            HTMLString.Append("< div class='Nome'>");

            HTMLString.Append("<b>Gabriel Gatto</b>");
            HTMLString.Append("</div>");
            HTMLString.Append("< div class='Curso'>");

            HTMLString.Append("<b>Pela conclusão do curso Curso Teste por meio da plataforma de aprendizagem virtual KD(Knowkedge Database).</b>");
            HTMLString.Append("</div>");
            HTMLString.Append("< div class='Instrutor'>");

            HTMLString.Append("<b>Instrutor Gabriel Roberto Gatto da Silva Sauro</b>");
            HTMLString.Append(" </div>    ");
            HTMLString.Append("< div class='Aluno'>");

            HTMLString.Append("<b>Aluno Gabriel Roberto Gatto da Silva Sauro</b>");
            HTMLString.Append("</div>   ");
            HTMLString.Append("< div class='Data'>");

            HTMLString.Append("<b>11/09/2016</b>");
            HTMLString.Append("</div>");
            HTMLString.Append("< div class='LabelData'>");

            HTMLString.Append("<b>Data</b>");
            HTMLString.Append("</div>");
            HTMLString.Append("</div>");

            string css = @".Nome{padding:320px 0 50px;font-family:'Kunstler Script';font-size:400%;color:#036;text-align:center;position:absolute;width:500px;height:535px;overflow-y:hidden;}.Curso{padding:0px 0 0 20px;font-family:'Arial';color:#036;font-size:100%;text-align:center;position:absolute;width:580px;height:60px;overflow-y:hidden;}.Instrutor{padding:127px 0 0;font-family:'Arial';color:#036;font-size:100%;text-align:center;position:absolute;width:410px}.Aluno{padding:100px 0 0;font-family:'Arial';color:#036;font-size:100%;text-align:center;position:absolute;width:410px}.Data{padding:57px 0 0;font-family:'Freestyle Script';color:#036;font-size:200%;text-align:center;position:absolute;width:320px}.LabelData{padding:0;font-family:'Arial';color:#036;font-size:100%;text-align:center;position:absolute;width:320px}"; 

            if (RequestExtensions.IsMobileBrowser(Request.UserAgent))
            {
                return new PdfActionResult(curso, (writer, document) =>
                {
                    document.Open();
                    document.SetPageSize(PageSize.A4);
                    document.NewPage();
                    Image imagem = Image.GetInstance(Server.MapPath(@"..\..\Images\bg-certificado.jpg"));
                    imagem.ScaleAbsolute(PageSize.A4);
                    imagem.SetAbsolutePosition(0, 0);
                    document.Add(imagem);

                    using (var msCss = new MemoryStream(Encoding.UTF8.GetBytes(css)))
                    {
                        using (var msHtml = new MemoryStream(Encoding.UTF8.GetBytes(HTMLString.ToString())))
                        {
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, msCss);
                        }
                    }
                    document.Close();

                })
                {
                    FileDownloadName = "Certificado.pdf"
                };
            }
            else
            {
                return new PdfActionResult(curso, (writer, document) =>
                {
                    document.Open();
                    document.SetPageSize(PageSize.A4);
                    document.NewPage();
                    Image imagem = Image.GetInstance(Server.MapPath(@"..\..\Images\bg-certificado.jpg"));
                    imagem.ScaleAbsolute(PageSize.A4);
                    imagem.SetAbsolutePosition(0, 0);
                    document.Add(imagem);

                    using (var msCss = new MemoryStream(Encoding.UTF8.GetBytes(css)))
                    {
                        using (var msHtml = new MemoryStream(Encoding.UTF8.GetBytes(HTMLString.ToString())))
                        {
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, msCss);
                        }
                    }
                    document.Close();
                });
            }
        }

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

            var detalhesCurso = Mapper.Map<Curso, DetalhesCurso>(curso);

            detalhesCurso.NotaCursoAtual = await db.NotaCurso.FindAsync(UsuarioLogado.Idusuario, detalhesCurso.Idcurso);

            ViewBag.UnidadeSelecionada = Idunidade;  //para reabrir o conteúdo

            //Verificar se é instrutor
            var cursoVerify = await db.Curso
                .Where(c => c.IdusuarioInstrutor == UsuarioLogado.Idusuario && c.Idcurso == id)
                .SingleOrDefaultAsync();

            detalhesCurso.IsInstutor = cursoVerify != null ? true : false;

            detalhesCurso.DisponibilizarCertificado = await VerificarCertificado(detalhesCurso);

            return View(detalhesCurso);
        }

        private async Task<bool> VerificarCertificado(DetalhesCurso detalhesCurso)
        {
            if (detalhesCurso.Dtfinal < DateTime.Today)
            {
                var usuario = await db.Usuario.FindAsync(UsuarioLogado.Idusuario);

                var notasDoUsuarioNoCurso = usuario.Notas
                    .Where(n => n.Atividade.Unidade.Idcurso == detalhesCurso.Idcurso);

                int notaUsuario = 0;
                foreach (var nota in notasDoUsuarioNoCurso)
                {
                    notaUsuario += nota.Atividade.Valor;
                }

                if ((notaUsuario * 100 / detalhesCurso.NotaMaxima) < 70)
                     return false;

                var materiaisConsultadosNoCurso = usuario.MateriaisConsultados
                    .Where(m => m.Unidade.Idcurso == detalhesCurso.Idcurso);

                int totalMateriaisCurso = 0;
                foreach (var unidade in detalhesCurso.Unidades)
                {
                    totalMateriaisCurso += unidade.Materiais.Count();
                }
                if (materiaisConsultadosNoCurso.Count() == totalMateriaisCurso)
                {
                    return true;
                }
            }
            return false;
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
                        var user = cursoToUpdate.Usuarios.Where(x => x.Iddepartamento == departamento.Iddepartamento).FirstOrDefault();
                        if (user == null)
                            cursoToUpdate.Departamentos.Remove(departamento);
                        else
                            cursoToUpdate.departamentosQueJaContemInscritos.Add(user.Departamento);
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
            db.Entry(curso).Collection("Departamentos").Load(); //Para remover também a referência
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
