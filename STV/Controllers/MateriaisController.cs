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
using System.IO;
using STV.DAL;
using STV.Auth;
using STV.ViewModels;
using AutoMapper;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace STV.Controllers
{
    public class MateriaisController : Controller
    {
        private STVDbContext db = new STVDbContext();

        private Usuario UsuarioLogado;

        public MateriaisController()
        {
            SessionContext auth = new SessionContext();
            UsuarioLogado = auth.GetUserData();
        }

        // GET: Materiais
        public async Task<ActionResult> Index(int idunidade = 0)
        {
            if (idunidade != 0)
            {
                var materiais = from m in db.Material where m.Unidade.Idunidade == idunidade select m;
                return PartialView(await materiais.ToListAsync());
            }
            else
            {
                var material = db.Material.Include(m => m.Unidade);
                return PartialView(await material.ToListAsync());
            }
        }

        private static string GetText(object e)
        {
            FieldInfo fieldInfo = e.GetType().GetField(e.ToString());
            DisplayAttribute[] displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            return null != displayAttributes && displayAttributes.Length > 0 ? displayAttributes[0].Name : e.ToString();
        }

        // GET: Materiais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                //Material material = await db.Material.Include(m => m.Arquivo.Nome).SingleOrDefaultAsync(m => m.Idmaterial == id);
                Material material = await db.Material.FindAsync(id);
                ViewBag.DescricaoTipo = GetText(material.Tipo);

                if (material == null)
                    throw new ApplicationException("Material não encontrado.");

                GetArquivoInfo(ref material);

                return View(material);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        private void GetArquivoInfo(ref Material material)
        {
            int Idmaterial = material.Idmaterial;

            var arquivoInfo = db.Arquivo.Where(a => a.Idmaterial == Idmaterial)
                .Select(a => new
                {
                    Idmaterial = a.Idmaterial,
                    Nome = a.Nome,
                    ContentType = a.ContentType,
                    Tamanho = a.Tamanho
                }).FirstOrDefault();

            if (arquivoInfo != null)
            {
                material.Arquivo = new Arquivo
                {
                    Nome = arquivoInfo.Nome,
                    Idmaterial = arquivoInfo.Idmaterial,
                    ContentType = arquivoInfo.ContentType,
                    Tamanho = arquivoInfo.Tamanho
                };
            }
        }

        // GET: Tipo
        public ActionResult CarregarTipo(int Idtipo, string url)
        {
            ViewBag.Tipo = (TipoMaterial)Idtipo;
            ViewBag.URL = url;
            return PartialView("Upload");
        }

        private void RegistrarVisualizacao(Material material)
        {
            var usuarioToUpdate = db.Usuario
                    .Include(u => u.MateriaisConsultados)
                    .Where(i => i.Idusuario == UsuarioLogado.Idusuario)
                    .Single();

            if (!usuarioToUpdate.MateriaisConsultados.Contains(material))
            {
                usuarioToUpdate.MateriaisConsultados.Add(material);
                db.SaveChanges();
            }
        }

        // GET: Tipo
        public async Task<ActionResult> MostrarArquivo(int Id)
        {
            var material = await db.Material.FindAsync(Id);

            if (material != null)
                RegistrarVisualizacao(material);
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GetArquivoInfo(ref material);

            switch (material.Tipo)
            {
                case TipoMaterial.Link:
                    break;

                case TipoMaterial.Imagem:
                    var blobArquivo = await db.Arquivo.Where(a => a.Idmaterial == Id)
                        .Select(a => new
                        {
                            Blob = a.Blob
                        }).SingleAsync();
                    material.Arquivo.Blob = blobArquivo.Blob;
                    break;

                default:
                    break;
            }
            return PartialView("ConteudoArquivo", material);
        }

        public FileResult BaixarArquivo(int Id)
        {

            var material = db.Material.Find(Id);

            if (material != null)
                RegistrarVisualizacao(material);

            var blobArquivo = db.Arquivo.Where(a => a.Idmaterial == Id)
                .Select(a => new
                {
                    Blob = a.Blob,
                    Nome = a.Nome,
                    ContentType = a.ContentType
                }).Single();

            Response.AppendHeader("Content-Disposition", "inline; filename=" + blobArquivo.Nome);
            return File(blobArquivo.Blob, blobArquivo.ContentType);
        }


        // GET: Materiais/Create
        public ActionResult Create(int Idunidade)
        {
            Material material = new Material();
            var materialVM = Mapper.Map<Material, MaterialVM>(material);
            material.Idunidade = Idunidade;
            return View(materialVM);
        }

        // POST: Materiais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idmaterial,Idunidade,Descricao,Tipo,URL")] MaterialVM materialVM, HttpPostedFileBase upload)
        {
            var material = Mapper.Map<MaterialVM, Material>(materialVM);

            if (ModelState.IsValid)
            {
                //using (var dbContextTransaction = db.Database.BeginTransaction())
                //{
                //    try
                //    {

                db.Material.Add(material);

                if (material.Tipo == TipoMaterial.Arquivo || material.Tipo == TipoMaterial.Imagem || material.Tipo == TipoMaterial.Video)
                {
                    GetUploadInfo(ref material, upload);

                    await db.SaveChangesAsync();

                    var arquivo = await db.Arquivo.FindAsync(material.Idmaterial);

                    //Grava o conteúdo do arquivo no banco de dados
                    VarbinaryStream blob = new VarbinaryStream(
                    db.Database.Connection.ConnectionString,
                    "Arquivo",
                    "Blob",
                    "Idmaterial",
                    arquivo.Idmaterial);

                    await upload.InputStream.CopyToAsync(blob, 65536);

                    //dbContextTransaction.Commit();
                }
                else
                {
                    await db.SaveChangesAsync();
                }

                TempData["msg"] = "Dados salvos!";

                return VoltarParaListagem(material);
                //    }
                //    catch (Exception)
                //    {
                //        dbContextTransaction.Rollback();
                //        throw;
                //    }
                //}
            }

            return View(material);
        }

        // GET: Materiais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Material material = await db.Material.FindAsync(id);

                if (material == null)
                    throw new ApplicationException("Material não encontrado.");

                GetArquivoInfo(ref material);

                ViewBag.URL = material.URL;
                ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo");

                return View(material);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Materiais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idmaterial,Idunidade,Descricao,Tipo,URL")] Material material, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                //using (var transaction = db.Database.BeginTransaction())
                //{
                //    try
                //    {

                GetUploadInfo(ref material, upload);

                if (material.Arquivo != null) db.Arquivo.Add(material.Arquivo);

                db.Entry(material).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (material.Arquivo != null)
                {
                    var arquivo = await db.Arquivo.FindAsync(material.Idmaterial);

                    //Grava o conteúdo do arquivo no banco de dados
                    VarbinaryStream blob = new VarbinaryStream(
                    db.Database.Connection.ConnectionString,
                    "Arquivo",
                    "Blob",
                    "Idmaterial",
                    arquivo.Idmaterial);

                    await upload.InputStream.CopyToAsync(blob, 65536);
                }

                //transaction.Commit();
                TempData["msg"] = "Dados salvos!";

                return VoltarParaListagem(material);
                //}

                //catch (Exception ex)
                //{
                //    transaction.Rollback();
                //    ModelState.AddModelError("", ex.Message);
                //    ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo");
                //}
                //}
            }
            return View(material);
        }


        // GET: Materiais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    throw new ApplicationException("Ops! Requisição inválida.");

                Material material = await db.Material.FindAsync(id);
                if (material == null)
                    throw new ApplicationException("Material não econtrado.");

                return View(material);
            }
            catch (ApplicationException ex)
            {
                TempData["msgErr"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Materiais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Material material = await db.Material.FindAsync(id);
            try
            {
                db.Entry(material).Collection("UsuariosConsulta").Load(); //Para remover também a referência
                db.Material.Remove(material);
                await db.SaveChangesAsync();
                TempData["msg"] = "Material excluído!";
                return VoltarParaListagem(material);
            }
            catch (Exception)
            {
                TempData["msgErr"] = "Material não pode ser excluído.";
                return RedirectToAction("Details", "Cursos", new { id = material.Unidade.Idcurso, Idunidade = material.Unidade.Idunidade });
            }
        }

        private void GetUploadContent(ref Material material, HttpPostedFileBase upload)
        {
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var arquivo = new Arquivo
                    {
                        Nome = Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        Idmaterial = material.Idmaterial
                    };

                    using (BinaryReader b = new BinaryReader(upload.InputStream))
                    {
                        byte[] filedata = b.ReadBytes((int)upload.InputStream.Length);
                        material.Arquivo.Blob = filedata;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> ExcluirArquivo(int id)
        {
            HttpStatusCodeResult HttpResult;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    await db.Database.ExecuteSqlCommandAsync(@"DELETE FROM [Arquivo] WHERE Idmaterial = {0}", id);

                    var material = await db.Material.FindAsync(id);
                    material.Tipo = TipoMaterial.Nenhum;
                    db.Entry(material).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    HttpResult = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    ViewBag.MensagemErro = ex.Message;
                    return HttpResult;
                }
            }
            HttpResult = new HttpStatusCodeResult(HttpStatusCode.OK);
            return HttpResult;
        }

        private void GetUploadInfo(ref Material material, HttpPostedFileBase upload)
        {
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    //teste myme
                    string myme = upload.ContentType;
                    if (Path.GetExtension(upload.FileName) == "docx")
                    {
                        myme = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    }

                    var arquivo = new Arquivo
                    {
                        Nome = Path.GetFileName(upload.FileName),
                        ContentType = myme,
                        Idmaterial = material.Idmaterial,
                        Tamanho = upload.ContentLength
                    };

                    material.Arquivo = arquivo;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Retorna para a tela principal do Curso
        private RedirectToRouteResult VoltarParaListagem(Material material)
        {
            Unidade unidade = db.Unidade.Find(material.Idunidade);
            return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso, Idunidade = material.Idunidade });
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
