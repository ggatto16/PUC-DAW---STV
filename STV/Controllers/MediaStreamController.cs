using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Web;
using STV.Models;
using System.Data;
using STV.DAL;

namespace STV.Controllers
{
    public class MediaStreamController : ApiController
    {

        private STVDbContext db = new STVDbContext();

        public HttpResponseMessage Get(int id)
        {
            string cs = db.Database.Connection.ConnectionString;
            string local = "0";
            try
            {

                //Recuperar informações do arquivo
                var arquivoInfo = db.Arquivo.Where(a => a.Idmaterial == id)
                    .Select(a => new
                    {
                        Idmaterial = a.Idmaterial,
                        Nome = a.Nome,
                        ContentType = a.ContentType,
                        Tamanho = a.Tamanho
                    }).Single();
                local = "1";
                VarbinaryStream filestream = new VarbinaryStream(
                                                    cs,
                                                    "Arquivo",
                                                    "Blob",
                                                    "Idmaterial",
                                                    id,
                                                    null,
                                                    true);
                local = "2";

                var response = Request.CreateResponse();

                local = "3";

                response.Content = new PushStreamContent(
                     async (Stream outputStream, HttpContent content, TransportContext context) =>
                        {
                            try
                            {
                                var buffer = new byte[65536];

                                using (Stream stream = filestream)
                                {
                                    local = "4";
                                    var length = (int)arquivoInfo.Tamanho;
                                    var bytesRead = 1;

                                    local = "5";
                                    while (length > 0 && bytesRead > 0)
                                    {
                                        bytesRead = stream.Read(buffer, 0, Math.Min(length, buffer.Length));
                                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                                        length -= bytesRead;
                                    }
                                    local = "6";
                                }

                            }
                            catch (HttpException)
                            {
                                return;
                            }
                            finally
                            {
                                outputStream.Close();
                            }
                        }, new MediaTypeHeaderValue(arquivoInfo.ContentType));

                return response;
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.Message + " - StackTrace = " + ex.StackTrace + "MinhaCS= " + cs);
                return response;
            }
        }

    }
}
