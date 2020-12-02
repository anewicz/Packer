using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Packer_v2.Startup))]
namespace Packer_v2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
