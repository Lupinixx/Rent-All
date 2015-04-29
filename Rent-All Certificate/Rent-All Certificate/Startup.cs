using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rent_All_Certificate.Startup))]
namespace Rent_All_Certificate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
