using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace STV.Controllers
{
    public class ArquivosController : Controller
    {
        private ModeloDados db = new ModeloDados();

        // GET: Arquivos
        public ActionResult Index(int id)
        {
            //          var fileToRetrieve = db.Arquivo.Find(id);
            //            return File(fileToRetrieve, fileToRetrieve.ContentType);
            return View();
        }

        [HttpDelete]
        public async Task<ActionResult> Excluir(int id)
        {
            HttpStatusCodeResult HttpResult;
            try
            {
                Arquivo arquivo = db.Arquivo.Find(id);
                db.Arquivo.Remove(arquivo);
                await db.SaveChangesAsync();

                HttpResult = new HttpStatusCodeResult(HttpStatusCode.OK);
                return HttpResult;
            }
            catch (Exception ex)
            {
                HttpResult = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                ViewBag.MensagemErro = ex.Message;
                return HttpResult;
            }
        }

    }
}