using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OracleTest.Startup))]
namespace OracleTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
