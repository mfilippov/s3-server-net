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

            var fileSystemProvider = Mock.Of<IFilesystemProvider>(pr =>
                pr.GetDirectories() == buckets.Select(b => b.Name).ToList() &&
                pr.GetDirectoryCreationTime(buckets[0].Name) == buckets[0].CreationDate &&
                pr.GetDirectoryCreationTime(buckets[1].Name) == buckets[1].CreationDate);
            
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
