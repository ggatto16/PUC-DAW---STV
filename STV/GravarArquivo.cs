using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebBackgrounder.DemoWeb
{
    public class GravarArquivo : Job
    {
        public GravarArquivo(TimeSpan interval, TimeSpan timeout)
            : base("GravarArquivo", interval, timeout)
        {
        }

        public override Task Execute()
        {
            return new Task(() => Thread.Sleep(3000));
        }
    }
}