using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PageTest.Startup))]
namespace PageTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
