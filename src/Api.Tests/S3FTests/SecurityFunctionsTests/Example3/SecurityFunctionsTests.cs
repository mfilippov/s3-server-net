using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Api.Tests.S3FTests.SecurityFunctionsTests.Example3
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
            const string etalonStr = "GET\n/\nlifecycle=\nhost:examplebucket.s3.amazonaws.com\nx-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855\nx-amz-date:20130524T000000Z\n\nhost;x-amz-content-sha256;x-amz-date\ne3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
            Assert.Equal(etalonStr, S3F.CreateCanonicalRequest("GET", "/", "?lifecycle", _headers, SignedHeaders, string.Empty));
        }

        [Fact]
        public void CreateStringToSignTest()
        {
            const string etalon =
                "AWS4-HMAC-SHA256\n20130524T000000Z\n20130524/us-east-1/s3/aws4_request\n9766c798316ff2757b517bc739a67f6213b4ab36dd5da2f94eaebf79c77395ca";
            Assert.Equal(etalon, S3F.CreateStringToSign(RequestTimestamp, Region, S3F.CreateCanonicalRequest("GET", "/", "?lifecycle", _headers, SignedHeaders, string.Empty)));
        }

        [Fact]
        public void CreateSignatureTest()
        {
            const string signature = "fea454ca298b7da1c68078a5d1bdbfbbe0d65c699e0f91ac7a200a0136783543";
            Assert.Equal(signature,
                S3F.ComputeSignature(RequestTimestamp, AwsSecretAccessKey, Region,
                    S3F.CreateStringToSign(RequestTimestamp, Region,
                        S3F.CreateCanonicalRequest("GET", "/", "?lifecycle", _headers, SignedHeaders, string.Empty))));
        }

        [Fact]
        public void CreateAuthorizationHeaderTest()
        {
            const string etalon = "AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request,SignedHeaders=host;x-amz-content-sha256;x-amz-date,Signature=fea454ca298b7da1c68078a5d1bdbfbbe0d65c699e0f91ac7a200a0136783543";
            Assert.Equal(etalon,
                S3F.AssembleAuthorizationHeader(AwsAccessKeyId, RequestTimestamp, Region, SignedHeaders,
                    S3F.ComputeSignature(RequestTimestamp, AwsSecretAccessKey, Region,
                        S3F.CreateStringToSign(RequestTimestamp, Region,
                            S3F.CreateCanonicalRequest("GET", "/", "?lifecycle", _headers, SignedHeaders, string.Empty)))));
        }
    }
}
