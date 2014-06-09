using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Api.Domain;

namespace Api.Security
{
    public class BucketLordTemple : IBucketLordTemple
    {
        private readonly IList<BucketLord> _lords; 
        private readonly XmlSerializer _bucketLordListSerializer = new XmlSerializer(typeof (List<BucketLord>));

        public BucketLordTemple()
        {
            _lords = new List<BucketLord>();
            _lords =
                _bucketLordListSerializer.Deserialize(File.Open("bucketlords.xml", FileMode.Open)) as List<BucketLord>;
        }

        public BucketLord CallBucketLord(string name)
        {
            return _lords.SingleOrDefault(l => l.UserName == name);
        }

        public BucketLord FindLordByAccessKeyId(string accessKeyId)
        {
            return _lords.SingleOrDefault(l => l.AccessKeyID == accessKeyId);
        }
    }
}
