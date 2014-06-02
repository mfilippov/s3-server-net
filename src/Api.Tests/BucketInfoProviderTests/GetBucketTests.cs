using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Api.Buckets;
using Api.Filesystem;
using Moq;
using Xunit;

namespace Api.Tests.BucketInfoProviderTests
{
    public class GetBucketTests
    {
        [Fact]
        public void GetBucketListActsWell()
        {
            const string bucketName = "vxaitp8ocn";
            var bucketCreationDate = new DateTime(2014, 5, 5, 12, 0, 35);
            var metadataFilePath = Path.Combine(bucketName, "metadata.xml");

            var serializator = new XmlSerializer(typeof (BucketInfo));
            var bucketMetadata = new BucketInfo()
            {
                Name = bucketName,
                CreationDate = bucketCreationDate
            };

            var xmlStream = new MemoryStream();
            serializator.Serialize(xmlStream, bucketMetadata);
            xmlStream.Seek(0, SeekOrigin.Begin);            

            var fileSystemProvider = Mock.Of<IFilesystemProvider>(pr =>
                pr.ListRootDirectory(false, true) == new List<string>{bucketName} &&
                pr.Exists(metadataFilePath, true, true) == true &&
                pr.StreamOfFile(metadataFilePath) == xmlStream);

            var bucketInfoProvider = new BucketInfoProvider(fileSystemProvider);

            var result = bucketInfoProvider.GetBucketList();

            Assert.True(result.Any());
            Assert.Equal(1, result.Count);
            Assert.Equal(bucketName, result.First().Name);
            Assert.Equal(bucketCreationDate, result.First().CreationDate);
        }
    }
}
