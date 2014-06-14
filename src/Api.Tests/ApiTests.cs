using System;
using System.Collections.Generic;
using Api.Configuration;
using Api.Domain;
using Api.Filesystem;
using Api.Security;
using Moq;
using Nancy.Testing;
using Xunit;

namespace Api.Tests
{
    public class S3ModuleTests
    {
        [Fact]
        public void WelcomePageTest()
        {   //Arrange IFilesystemProvider instance
            const string bucketName = "vxaitp8ocn";
            var bucketCreationDate = new DateTime(2014, 5, 5, 12, 0, 35);

            var fileSystemProvider = Mock.Of<IFilesystemProvider>(pr =>
                pr.GetBucketList() == new List<string> { bucketName } &&
                pr.GetBucketCreationDateTime(bucketName) == bucketCreationDate);
            var nodeConfigutration = Mock.Of<INodeConfiguration>(nc => nc.NodeEndpoint == "test.s3.net");
            var bucketLordTemple = Mock.Of<IBucketLordTemple>(bt => bt.FindLordByAccessKeyId("AKIAIOSFODNN7EXAMPLE") == new BucketLord
            {
                Id = "bcaf1ffd86f461ca5fb16fd081034f",
                AccessKeyId = "AKIAIOSFODNN7EXAMPLE",
                SecretKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
                DisplayName = "TestLord"
            });
            //instace ready

            var bootstrapper = new TestNancyBootstrapper(nodeConfigutration, fileSystemProvider, bucketLordTemple);
            
            var browser = new Browser(bootstrapper);
            var result = browser.Get("/", context => context.Header("Authorization", "AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request,SignedHeaders=host;range;x-amz-content-sha256;x-amz-date,Signature=dc665cbcfda4616de1eb422d4b95a82a51047634af44a9da5ae7b77d0531ed09"));

           var etalonDocument = @"<?xml version=""1.0"" encoding=""utf-8""?>
          <ListAllMyBucketsResult>
            <Owner>
              <ID>bcaf1ffd86f461ca5fb16fd081034f</ID>
              <DisplayName>TestLord</DisplayName>
            </Owner>
            <Buckets>
              <Bucket>
                <Name>vxaitp8ocn</Name>
                <CreationDate>2014-05-05T12:00:35.000Z</CreationDate>
              </Bucket>
            </Buckets>
          </ListAllMyBucketsResult>".Replace("\r\n", "").Replace(" ", "");
            //TODO: Remove 0 i think it BOM problem.
           var resultDocument = result.Body.AsString().Replace("\r\n", "").Replace(" ", "").Remove(0, 1);
            Assert.Equal(etalonDocument, resultDocument);
        }
    }
}