using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var fileToRetrieve = db.Arquivo.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }

        [HttpDelete]
        public async Task<ActionResult> Excluir(int id)
        {
            Arquivo arquivo = db.Arquivo.Find(id);
            db.Arquivo.Remove(arquivo);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Materiais", new { id = arquivo.Idmaterial });
        }

    }
}