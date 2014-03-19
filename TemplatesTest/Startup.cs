using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TemplatesTest.Startup))]
namespace TemplatesTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
