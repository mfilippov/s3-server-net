using Nancy;
using Nancy.Testing;
using Xunit;

namespace Api.Tests
{
    public class S3ModuleTests
    {
        [Fact]
        public void WelcomePageTest()
        {
            var browser = new Browser(new DefaultNancyBootstrapper());
            Assert.Equal(HttpStatusCode.OK, browser.Get("/").StatusCode);
        }
    }
}