using Xunit;

namespace Api.Tests.S3FTests
{
    public class UriEncodeTests
    {
        [Fact]
        public void UriEncodeDoesNotChangeSimpleString()
        {
            Assert.Equal("abc", S3F.UriEncode("abc", false));
            Assert.Equal("abc", S3F.UriEncode("abc", true));
        }

        [Fact]
        public void UriEncodeReplacesSlashesOnlyWhenFlagIsSet()
        {
            Assert.Equal("a/b", S3F.UriEncode("a/b", false));
            Assert.Equal("a%2Fb", S3F.UriEncode("a/b", true));
        }

        [Fact]
        public void UriEncodeReplacesWhitespaceWithPercent20()
        {
            Assert.Equal("%20", S3F.UriEncode(" ", false));
            Assert.Equal("%20", S3F.UriEncode(" ", true));
        }

        [Fact]
        public void CheckEncode()
        {
            Assert.Equal("http%3A//meyerweb.com/eric/tools/dencoder/", S3F.UriEncode("http://meyerweb.com/eric/tools/dencoder/", false));
            Assert.Equal("http%3A%2F%2Fmeyerweb.com%2Feric%2Ftools%2Fdencoder%2F", S3F.UriEncode("http://meyerweb.com/eric/tools/dencoder/", true));
        }
    }
}
