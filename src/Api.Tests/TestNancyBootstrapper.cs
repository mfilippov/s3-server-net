using Api.Configuration;
using Api.Filesystem;
using Api.Security;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Api.Tests
{
    public class TestNancyBootstrapper : DefaultNancyBootstrapper
    {
        private readonly IFilesystemProvider _filesystemProvider;
        private readonly INodeConfiguration _nodeConfiguration;

        public TestNancyBootstrapper(IFilesystemProvider filesystemProvider)
        {
            _filesystemProvider = filesystemProvider;
        }

        public TestNancyBootstrapper(INodeConfiguration nodeConfiguration)
        {
            _nodeConfiguration = nodeConfiguration;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            if (_filesystemProvider != null) container.Register(typeof(IFilesystemProvider), _filesystemProvider);
            if (_nodeConfiguration != null) container.Register(typeof(INodeConfiguration), _nodeConfiguration);
        }

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
    }
}
