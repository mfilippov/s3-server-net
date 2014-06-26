using Xunit;

namespace Api.Tests.S3FTests
{
    public class CreateCanonicalQueryStringTests
    {
        [Fact]
        public void CorrectlyHandlesEmptyString()
        {
            Assert.Equal(string.Empty, S3F.CreateCanonicalQueryString(string.Empty));
        }

        [Fact]
        public void EmptyParameter()
        {
            Assert.Equal("lifecycle=", S3F.CreateCanonicalQueryString("?lifecycle"));
        }

        [Fact]
        public void CorrectQueryString()
        {
            Assert.Equal("max-keys=2&prefix=J", S3F.CreateCanonicalQueryString("?max-keys=2&prefix=J"));
        }
    }
}
