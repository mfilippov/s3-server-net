using Api.Filesystem;
using Nancy;
using Nancy.TinyIoc;

namespace Server
{
    public class S3ServerBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(typeof (IFilesystemProvider), new FilesystemProvider("."));
        }
    }
}
