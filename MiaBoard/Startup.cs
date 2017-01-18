using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MiaBoard.Startup))]
namespace MiaBoard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
