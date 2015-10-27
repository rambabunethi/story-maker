using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StoryMaker.Startup))]
namespace StoryMaker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
