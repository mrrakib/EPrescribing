using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EPrescribing.Web.Startup))]
namespace EPrescribing.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
