using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpertSystem.Startup))]
namespace ExpertSystem
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
