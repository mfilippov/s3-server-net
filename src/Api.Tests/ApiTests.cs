using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            var nodeConfigutration = Mock.Of<INodeConfiguration>();
            var bucketLordTemple = Mock.Of<IBucketLordTemple>(bt => bt.FindLordByAccessKeyId("AKIAIOSFODNN7EXAMPLE") == new BucketLord
            {
                AccessKeyId = "AKIAIOSFODNN7EXAMPLE",
                SecretKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
            });
            //instace ready

            var bootstrapper = new TestNancyBootstrapper(nodeConfigutration, fileSystemProvider, bucketLordTemple);
            
            var browser = new Browser(bootstrapper);
            var result = browser.Get("/", context => context.Header("Authorization", "AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request,SignedHeaders=host;range;x-amz-content-sha256;x-amz-date,Signature=84b900417fea8c1abdd64ad82cf5bc2b2ec547c15e13adb20e2d63226eb93a93"));

            const string etalonResponse = @"<?xml version=""1.0"" encoding=""utf-8""?>
          <ListAllMyBucketsResult>
            <Owner>
              <ID>bcaf1ffd86f461ca5fb16fd081034f</ID>
              <DisplayName>webfile</DisplayName>
            </Owner>
            <Buckets>
              <Bucket>
                <Name>vxaitp8ocn</Name>
                <CreationDate>12:00:35</CreationDate>
              </Bucket>
            </Buckets>
          </ListAllMyBucketsResult>";
            Assert.Equal(etalonResponse, result.Body.AsString().Substring(1));
        }
    }
}