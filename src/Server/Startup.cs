using Owin;

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
#if DEBUG
            app.UseErrorPage();
#endif
            app.UseWelcomePage("/");
        }
    }
}