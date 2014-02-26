using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsonTest.Startup))]
namespace JsonTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
