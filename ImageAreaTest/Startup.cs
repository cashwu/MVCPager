using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageAreaTest.Startup))]
namespace ImageAreaTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
