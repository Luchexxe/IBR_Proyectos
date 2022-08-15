using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(wsUPCServices.Startup))]

namespace wsUPCServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
