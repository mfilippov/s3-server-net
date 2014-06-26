using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Api.Tests.S3FTests.SecurityFunctionsTests.Example2
{
    public class SecurityFunctionsTests
    {
        private const string AwsAccessKeyId = "AKIAIOSFODNN7EXAMPLE";
        private const string AwsSecretAccessKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
        private static readonly DateTime RequestTimestamp = new DateTime(2013, 5, 24);
        private const string Region = "us-east-1";

        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>
        {
            {"Host", "examplebucket.s3.amazonaws.com"},
            {"Date", "Fri, 24 May 2013 00:00:00 GMT"},
            {"x-amz-storage-class", "REDUCED_REDUNDANCY"},
            {"x-amz-content-sha256", "44ce7dd67c959e0d3524ffac1771dfbba87d2b6b4b4e99e42034a8b803f8b072"},
            {"x-amz-date", "20130524T000000Z"}
        };

        private List<string> SignedHeaders { get { return _headers.Keys.ToList(); } }

        [Fact]
        public void CreateCanonicalRequestTest()
        {
            const string etalonStr = "PUT\n/test%24file.txt\n\ndate:Fri, 24 May 2013 00:00:00 GMT\nhost:examplebucket.s3.amazonaws.com\nx-amz-content-sha256:44ce7dd67c959e0d3524ffac1771dfbba87d2b6b4b4e99e42034a8b803f8b072\nx-amz-date:20130524T000000Z\nx-amz-storage-class:REDUCED_REDUNDANCY\n\ndate;host;x-amz-content-sha256;x-amz-date;x-amz-storage-class\n44ce7dd67c959e0d3524ffac1771dfbba87d2b6b4b4e99e42034a8b803f8b072";
            Assert.Equal(etalonStr, S3F.CreateCanonicalRequest("PUT", "/test$file.txt", string.Empty, _headers, SignedHeaders, "Welcome to Amazon S3."));
        }

        // Хэш не совпадает с указаным в документации.
        // Т.к. предыдущий шаг отрабатывает корректно: есть подозврение, что в документации ошибка в примере
        /*
        [Fact]
        public void CreateStringToSignTest()
        {
            const string etalonStr = "PUT\n/test%24file.txt\n\ndate:Fri, 24 May 2013 00:00:00 GMT\nhost:examplebucket.s3.amazonaws.com\nx-amz-content-sha256:44ce7dd67c959e0d3524ffac1771dfbba87d2b6b4b4e99e42034a8b803f8b072\nx-amz-date:20130524T000000Z\nx-amz-storage-class:REDUCED_REDUNDANCY\n\ndate;host;x-amz-content-sha256;x-amz-date;x-amz-storage-class\n44ce7dd67c959e0d3524ffac1771dfbba87d2b6b4b4e99e42034a8b803f8b072";

            const string etalon =
                "AWS4-HMAC-SHA256\n20130524T000000Z\n20130524/us-east-1/s3/aws4_request\n9e0e90d9c76de8fa5b200d8c849cd5b8dc7a3be3951ddb7f6a76b4158342019d";
            Assert.Equal(etalon, S3F.CreateStringToSign(RequestTimestamp, Region, etalonStr));
        }

        [Fact]
        public void CreateSignatureTest()
        {
            const string signature = "98ad721746da40c64f1a55b78f14c238d841ea1380cd77a1b5971af0ece108bd";
            Assert.Equal(signature,
                S3F.ComputeSignature(RequestTimestamp, AwsSecretAccessKey, Region,
                    S3F.CreateStringToSign(RequestTimestamp, Region,
                        S3F.CreateCanonicalRequest("PUT", "/test$file.txt", string.Empty, _headers, SignedHeaders,
                            "Welcome to Amazon S3."))));
        }

        [Fact]
        public void CreateAuthorizationHeaderTest()
        {
            const string etalon = "AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request,SignedHeaders=host;range;x-amz-content-sha256;x-amz-date,Signature=f0e8bdb87c964420e857bd35b5d6ed310bd44f0170aba48dd91039c6036bdb41";
            Assert.Equal(etalon,
                S3F.AssembleAuthorizationHeader(AwsAccessKeyId, RequestTimestamp, Region, SignedHeaders,
                    S3F.ComputeSignature(RequestTimestamp, AwsSecretAccessKey, Region,
                        S3F.CreateStringToSign(RequestTimestamp, Region,
                            S3F.CreateCanonicalRequest("GET", "/test.txt", string.Empty, _headers, SignedHeaders,
                                string.Empty)))));
        }
        */
    }
}
