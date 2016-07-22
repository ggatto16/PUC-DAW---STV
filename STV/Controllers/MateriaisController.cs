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
namespace STV.Controllers
{
    public class MateriaisController : Controller
    {
        private STVDbContext db = new STVDbContext();

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

        // GET: Materiais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Material material = await db.Material.Include(m => m.Arquivo.Nome).SingleOrDefaultAsync(m => m.Idmaterial == id);
            Material material = await db.Material.FindAsync(id);

            if (material == null)
            {
                return HttpNotFound();
            }

            GetArquivoInfo(ref material);

            return View(material);
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
        public ActionResult CarregarTipo(int Idtipo)
        {
            return PartialView("Upload");
        }

        // GET: Tipo
        public async Task<ActionResult> MostrarVideo(int Id)
        {
            var material = await db.Material.FindAsync(Id);

            GetArquivoInfo(ref material);

            if (material.Tipo == TipoMaterial.Imagem)
            {
                var blobArquivo = await db.Arquivo.Where(a => a.Idmaterial == Id)
                    .Select(a => new
                    {
                        Blob = a.Blob
                    }).SingleAsync();
                material.Arquivo.Blob = blobArquivo.Blob;
            }

            return PartialView("ConteudoArquivo", material);
        }

        // GET: Materiais/Create
        public ActionResult Create(int Idunidade)
        {
            ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo");
            Material material = new Material();
            material.Idunidade = Idunidade;
            return View(material);
        }

        // POST: Materiais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Idmaterial,Idunidade,Descricao,Tipo")] Material material, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //using (var transaction = db.Database.BeginTransaction())
                //{
                //    try
                //    {
                        db.Material.Add(material);
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
                        //transaction.Commit();

                        return VoltarParaListagem(material);
                //    }
                //    catch (Exception)
                //    {
                //        transaction.Rollback();
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
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Material material = await db.Material.FindAsync(id);

                if (material == null) return HttpNotFound();

                GetArquivoInfo(ref material);

                ViewBag.Idunidade = new SelectList(db.Unidade, "Idunidade", "Titulo");

                return View(material);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Materiais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Idmaterial,Idunidade,Descricao,Tipo")] Material material, HttpPostedFileBase upload)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = await db.Material.FindAsync(id);
            if (material == null)
            {
                return HttpNotFound();
            }

            return View(material);
        }

        // POST: Materiais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Material material = await db.Material.FindAsync(id);
            db.Material.Remove(material);
            await db.SaveChangesAsync();
            return VoltarParaListagem(material);
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

                    //using (var fileData = new MemoryStream())
                    //{
                    //    upload.InputStream.CopyTo(fileData);
                    //    arquivo.Blob = fileData.ToArray();
                    //}

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
                    var arquivo = new Arquivo
                    {
                        Nome = Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
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
            try
            {
                Unidade unidade = db.Unidade.Find(material.Idunidade);
                return RedirectToAction("Details", "Cursos", new { id = unidade.Idcurso, Idunidade = material.Idunidade });
            }
            catch (Exception)
            {
                throw;
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
