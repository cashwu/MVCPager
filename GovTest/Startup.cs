using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GovTest.Startup))]
namespace GovTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
