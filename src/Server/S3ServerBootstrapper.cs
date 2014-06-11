using Api.Configuration;
using Api.Security;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Server
{
    public class S3ServerBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            var securityCfg = new StatelessAuthenticationConfiguration(ctx =>
            {
                var userValidator =
                    container.Resolve<IFaceControlService>();

                return userValidator.CheckAuth(ctx.Request);
            });
            StatelessAuthentication.Enable(pipelines, securityCfg);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(typeof (INodeConfiguration), new NodeConfiguration());
        }
    }
}
