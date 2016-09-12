using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmDmAnalogProject.Startup))]
namespace AmDmAnalogProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
