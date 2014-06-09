using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Api.Domain;
using Api.Filesystem;
using Moq;
using Nancy.Testing;
using Xunit;

namespace Api.Tests
{
    public class S3ModuleTests
    {
        [Fact]
        public void WelcomePageTest()
        {
            //Arrange IFilesystemProvider instance
            const string bucketName = "vxaitp8ocn";
            var bucketCreationDate = new DateTime(2014, 5, 5, 12, 0, 35);
            var metadataFilePath = Path.Combine(bucketName, "metadata.xml");

            var serializator = new XmlSerializer(typeof(BucketInfo));
            var bucketMetadata = new BucketInfo
            {
                Name = bucketName,
                CreationDate = bucketCreationDate
            };

            var xmlStream = new MemoryStream();
            serializator.Serialize(xmlStream, bucketMetadata);
            xmlStream.Seek(0, SeekOrigin.Begin);

            var fileSystemProvider = Mock.Of<IFilesystemProvider>(pr =>
                pr.ListRootDirectory(false, true) == new List<string> { bucketName } &&
                pr.Exists(metadataFilePath, true, true) == true &&
                pr.StreamOfFile(metadataFilePath) == xmlStream);
            //instace ready

            var bootstrapper = new TestNancyBootstrapper(fileSystemProvider);

            var browser = new Browser(bootstrapper);
            var result = browser.Get("/");

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