using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Api.Tests.S3FTests
{
    public class SecurityFunctionsTests
    {
        private static string AWSAccessKeyId = "AKIAIOSFODNN7EXAMPLE";
        private static string AWSSecretAccessKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
        private static DateTime RequestTimestamp = new DateTime(2013, 5, 24);
        private static string Region = "us-east-1";

        private readonly SortedDictionary<string, string> _headers = new SortedDictionary<string, string>
        {
            {"Host", " examplebucket.s3.amazonaws.com"},
            {"Range", " bytes=0-9"},
            {"x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"},
            {"x-amz-date", "20130524T000000Z"}
        };

        private List<string> SignedHeaders { get { return _headers.Keys.ToList(); } }

        [Fact]
        public void CreateCanonicalRequestTest()
        {
            const string etalonStr = "GET\n/test.txt\n\nhost:examplebucket.s3.amazonaws.com\nrange:bytes=0-9\nx-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855\nx-amz-date:20130524T000000Z\n\nhost;range;x-amz-content-sha256;x-amz-date\ne3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
            Assert.Equal(etalonStr, S3F.CreateCanonicalRequest("GET", "/test.txt", string.Empty, _headers, SignedHeaders, string.Empty));
        }

        [Fact]
        public void CreateSigningKeyTest()
        {
            const string etalon =
                "AWS4-HMAC-SHA256\n20130524T000000Z\n20130524/us-east-1/s3/aws4_request\n7344ae5b7ee6c3e7e6b0fe0640412a37625d1fbfff95c48bbb2dc43964946972";
            Assert.Equal(etalon, S3F.CreateStringToSign(RequestTimestamp, Region, S3F.CreateCanonicalRequest("GET", "/test.txt", string.Empty, _headers, SignedHeaders, string.Empty)));
        }
    }
}
