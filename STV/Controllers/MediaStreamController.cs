using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STV.Media;
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

        private ModeloDados db = new ModeloDados();

        public HttpResponseMessage Get(int id)
        {

            //Recuperar informações do arquivo
            var arquivoInfo = db.Arquivo.Where(a => a.Idmaterial == id)
                .Select(a => new {
                    Idmaterial = a.Idmaterial,
                    Nome = a.Nome,
                    ContentType = a.ContentType,
                    Tamanho = a.Tamanho
                }).Single();

            VarbinaryStream filestream = new VarbinaryStream(
                                                db.Database.Connection.ConnectionString,
                                                "Arquivo",
                                                "Blob",
                                                "Idmaterial",
                                                id,
                                                true);

            var response = Request.CreateResponse();

            response.Content = new PushStreamContent(
                 async (Stream outputStream, HttpContent content, TransportContext context) =>
                    {
                        try
                        {
                            var buffer = new byte[65536];

                            using (Stream stream = filestream)
                            {
                                var length = (int)arquivoInfo.Tamanho;
                                var bytesRead = 1;


                                while (length > 0 && bytesRead > 0)
                                {
                                    bytesRead = stream.Read(buffer, 0, Math.Min(length, buffer.Length));
                                    await outputStream.WriteAsync(buffer, 0, bytesRead);
                                    length -= bytesRead;
                                }
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

    }
}
