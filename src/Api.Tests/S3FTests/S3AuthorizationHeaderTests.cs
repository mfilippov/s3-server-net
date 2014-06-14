using System;
using System.Collections.Generic;
using Api.Security;
using Xunit;

namespace Api.Tests.S3FTests
{
    public class S3AuthorizationHeaderTests
    {
        [Fact]
        public void ParseTest() 
        {
            const string etalon = "AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request,SignedHeaders=host;range;x-amz-content-sha256;x-amz-date,Signature=f0e8bdb87c964420e857bd35b5d6ed310bd44f0170aba48dd91039c6036bdb41";
            var result = S3AuthorizationHeader.ParseHeader(etalon);
            Assert.Equal("AWS4-HMAC-SHA256", result.HashAlgorithm);
            Assert.Equal("AKIAIOSFODNN7EXAMPLE", result.Credentials.AccessKeyId);
            Assert.Equal(new DateTime(2013, 5, 24), result.Credentials.Date);
            Assert.Equal("us-east-1", result.Credentials.Region);
            Assert.Equal("s3", result.Credentials.Service);
            Assert.Equal(new List<string> { "host","range", "x-amz-content-sha256", "x-amz-date" }, result.SignedHeaders);
            Assert.Equal("f0e8bdb87c964420e857bd35b5d6ed310bd44f0170aba48dd91039c6036bdb41", result.Signature);
        }
    }
}
