using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StepMap.WebClient.Startup))]
namespace StepMap.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
