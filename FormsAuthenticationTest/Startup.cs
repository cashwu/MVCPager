using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FormsAuthenticationTest.Startup))]
namespace FormsAuthenticationTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
