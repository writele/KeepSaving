using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KeepSaving.Startup))]
namespace KeepSaving
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
