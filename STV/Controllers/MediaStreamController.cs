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

namespace STV.Controllers
{
    public class MediaStreamController : ApiController
    {
        private string _filename;


        public HttpResponseMessage Get(string filename, string ext)
        {
            var video = new MediaStream(filename, ext);

            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(
                 async (Stream outputStream, HttpContent content, TransportContext context) =>
                    {
                        try
                        {
                            var buffer = new byte[65536];

                            using (var media = File.Open(_filename, FileMode.Open, FileAccess.Read))
                            {
                                var length = (int)media.Length;
                                var bytesRead = 1;

                                while (length > 0 && bytesRead > 0)
                                {
                                    bytesRead = media.Read(buffer, 0, Math.Min(length, buffer.Length));
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
                    }, new MediaTypeHeaderValue("video/" + ext));

            return response;
        }

    }
}
