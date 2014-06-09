using Api.Configuration;
using Api.Filesystem;
using Nancy;
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
    }
}
