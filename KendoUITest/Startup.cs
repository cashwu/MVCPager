using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KendoUITest.Startup))]
namespace KendoUITest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
