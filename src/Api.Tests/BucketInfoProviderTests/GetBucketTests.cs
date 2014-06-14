using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Api.Buckets;
using Api.Domain;
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
            var buckets = new List<BucketInfo>
            {
                new BucketInfo
                {
                    Name = "quotes",
                    CreationDate = new DateTime(2006, 02, 03, 16, 45, 09, 0)
                },
                new BucketInfo
                {
                    Name = "samples",
                    CreationDate = new DateTime(2006, 02, 03, 16, 42, 58, 0)
                }
            };

            Func<string, string> makePath = s => Path.Combine(s, "metadata.xml");

            var serializator = new XmlSerializer(typeof (BucketInfo));

            Func<BucketInfo, Stream> makeStream = bi =>
            {
                var xmlStream = new MemoryStream();
                serializator.Serialize(xmlStream, bi);
                xmlStream.Seek(0, SeekOrigin.Begin);
                return xmlStream;
            }; 

            var fileSystemProvider = Mock.Of<IFilesystemProvider>(pr =>
                pr.ListRootDirectory(false, true) == buckets.Select(b => b.Name).ToList());
            Mock.Get(fileSystemProvider)
                .Setup(m => m.Exists(It.IsAny<string>()))
                .Returns((string s) => buckets.Select(b => makePath(b.Name)).Contains(s));
            Mock.Get(fileSystemProvider)
                .Setup(m => m.StreamOfFile(It.IsAny<string>()))
                .Returns((string s) => makeStream(buckets.First(b => makePath(b.Name) == s)));

            var bucketInfoProvider = new BucketInfoProvider(fileSystemProvider);

            var result = bucketInfoProvider.GetBucketList();

            Assert.True(result.Any());
            Assert.Equal(2, result.Count);
            Assert.Equal(buckets[0].Name, result[0].Name);
            Assert.Equal(buckets[0].CreationDate, result[0].CreationDate);
            Assert.Equal(buckets[1].Name, result[1].Name);
            Assert.Equal(buckets[1].CreationDate, result[1].CreationDate);
        }
    }
}
