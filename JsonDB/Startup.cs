using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsonDB.Startup))]
namespace JsonDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
