using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(STV.Startup))]
namespace STV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
