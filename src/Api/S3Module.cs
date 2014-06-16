using System.Linq;
using System.Xml.Linq;
using Api.Buckets;
using Api.Domain;
using Api.Infrastructure;
using Nancy;
using Nancy.Security;

namespace Api
{
    public class S3Module : NancyModule
    {
        public S3Module(IBucketInfoProvider bucketInfoProvider)
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var owner = (BucketLord) Context.CurrentUser;
                var buckets = bucketInfoProvider.GetBucketList();

                var document = new XDocument(
                    new XElement("ListAllMyBucketsResult",
                        new XElement("Owner",
                            new XElement("ID", owner.Id),
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
                return new XDocumentResponse(document);
            };
        }
    }
}