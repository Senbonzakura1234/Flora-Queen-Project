using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flora_Queen_Project.Startup))]
namespace Flora_Queen_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
