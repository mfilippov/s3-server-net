using System.IO;
using System.Linq;
using System.Xml.Linq;
using Api.Buckets;
using Api.Domain;
using Nancy;

namespace Api
{
    public class S3Module : NancyModule
    {
        public S3Module(IBucketInfoProvider bucketInfoProvider)
        {
            Get["/"] = _ =>
            {
                var owner = new Owner {ID = "bcaf1ffd86f461ca5fb16fd081034f", DisplayName = "webfile"};
                var buckets = bucketInfoProvider.GetBucketList();

                var document = new XDocument(
                    new XElement("ListAllMyBucketsResult",
                        new XElement("Owner",
                            new XElement("ID", owner.ID),
                            new XElement("DisplayName", owner.DisplayName)
                            ),
                        new XElement("Buckets", buckets.Select(b =>
                            new XElement("Bucket",
                                new XElement("Name", b.Name),
                                new XElement("CreationDate", b.CreationDate.ToS3String())
                                )
                            )
                            )
                        )
                    );
                var resultStream = new MemoryStream();
                document.Save(resultStream);
                resultStream.Seek(0, SeekOrigin.Begin);
                return Response.FromStream(resultStream, "text/xml");
            };
        }
    }
}