using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CKEditor.Startup))]
namespace CKEditor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
