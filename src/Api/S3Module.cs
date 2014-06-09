using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Api.Buckets;
using Api.Configuration;
using Api.Domain;
using Nancy;
using Nancy.Security;

namespace Api
{
    public class S3Module : NancyModule
    {
        public S3Module(INodeConfiguration appConfiguration, IBucketInfoProvider bucketInfoProvider)
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var owner = new BucketLord {ID = "bcaf1ffd86f461ca5fb16fd081034f", DisplayName = "webfile"};
                var buckets = bucketInfoProvider.GetBucketList();

                var date = Request.Headers.Date;

                var document = new XDocument(
                    new XElement("ListAllMyBucketsResult",
                        new XAttribute("xmlns", 
                            string.Format("http://{0}/doc/{1}", 
                            appConfiguration.NodeEndpoint, 
                            (date.HasValue ? date.Value : DateTime.Today).ToString("yyyy-MM-dd"))),
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