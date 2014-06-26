using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Api.Tests.S3FTests.SecurityFunctionsTests.Example4
{
    public class SecurityFunctionsTests
    {
        private const string AwsAccessKeyId = "AKIAIOSFODNN7EXAMPLE";
        private const string AwsSecretAccessKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
        private static readonly DateTime RequestTimestamp = new DateTime(2013, 5, 24);
        private const string Region = "us-east-1";

        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>
        {
            {"Host", " examplebucket.s3.amazonaws.com"},
            {"x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"},
            {"x-amz-date", "20130524T000000Z"}
        };

        private List<string> SignedHeaders { get { return _headers.Keys.ToList(); } }

        [Fact]
        public void CreateCanonicalRequestTest()
        {
            const string etalonStr = "GET\n/\nmax-keys=2&prefix=J\nhost:examplebucket.s3.amazonaws.com\nx-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855\nx-amz-date:20130524T000000Z\n\nhost;x-amz-content-sha256;x-amz-date\ne3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
            Assert.Equal(etalonStr, S3F.CreateCanonicalRequest("GET", "/", "?max-keys=2&prefix=J", _headers, SignedHeaders, string.Empty));
        }

        [Fact]
        public void CreateStringToSignTest()
        {
            const string etalon =
                "AWS4-HMAC-SHA256\n20130524T000000Z\n20130524/us-east-1/s3/aws4_request\ndf57d21db20da04d7fa30298dd4488ba3a2b47ca3a489c74750e0f1e7df1b9b7";
            Assert.Equal(etalon, S3F.CreateStringToSign(RequestTimestamp, Region, S3F.CreateCanonicalRequest("GET", "/", "?max-keys=2&prefix=J", _headers, SignedHeaders, string.Empty)));
        }

        [Fact]
        public void CreateSignatureTest()
        {
            const string signature = "34b48302e7b5fa45bde8084f4b7868a86f0a534bc59db6670ed5711ef69dc6f7";
            Assert.Equal(signature,
                S3F.ComputeSignature(RequestTimestamp, AwsSecretAccessKey, Region,
                    S3F.CreateStringToSign(RequestTimestamp, Region,
                        S3F.CreateCanonicalRequest("GET", "/", "?max-keys=2&prefix=J", _headers, SignedHeaders,
                            string.Empty))));
        }

        [Fact]
        public void CreateAuthorizationHeaderTest()
        {
            const string etalon = "AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request,SignedHeaders=host;x-amz-content-sha256;x-amz-date,Signature=34b48302e7b5fa45bde8084f4b7868a86f0a534bc59db6670ed5711ef69dc6f7";
            Assert.Equal(etalon,
                S3F.AssembleAuthorizationHeader(AwsAccessKeyId, RequestTimestamp, Region, SignedHeaders,
                    S3F.ComputeSignature(RequestTimestamp, AwsSecretAccessKey, Region,
                        S3F.CreateStringToSign(RequestTimestamp, Region,
                            S3F.CreateCanonicalRequest("GET", "/", "?max-keys=2&prefix=J", _headers, SignedHeaders,
                                string.Empty)))));
        }
    }
}
