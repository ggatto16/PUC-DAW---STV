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

namespace STV.Controllers
{
    public class MediaStreamController : ApiController
    {

        private ModeloDados db = new ModeloDados();

        public HttpResponseMessage Get(int id)
        {
            var arquivo = db.Arquivo.Find(id);

            var response = Request.CreateResponse();

            response.Content = new PushStreamContent(
                 async (Stream outputStream, HttpContent content, TransportContext context) =>
                    {
                        try
                        {
                            var buffer = new byte[65536];

                            using (Stream stream = new MemoryStream(arquivo.Blob))
                            {
                                var length = (int)stream.Length;
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
                    }, new MediaTypeHeaderValue(arquivo.ContentType));

            return response;
        }

    }
}
