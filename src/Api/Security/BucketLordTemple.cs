using System.Collections.Generic;
using System.Linq;
using Api.Configuration;
using Api.Domain;
using Api.Filesystem;
using Newtonsoft.Json;

namespace Api.Security
{
    public class BucketLordTemple : IBucketLordTemple
    {
        private readonly IList<BucketLord> _lords; 

        public BucketLordTemple(IFilesystemProvider filesystemProvider, INodeConfiguration nodeConfiguration)
        {
            _lords = JsonConvert.DeserializeObject<List<BucketLord>>(filesystemProvider.ReadToEnd(nodeConfiguration.BucketLordsFile));
        }

        public BucketLord CallBucketLord(string name)
        {
            return _lords.SingleOrDefault(l => l.UserName == name);
        }

        public BucketLord FindLordByAccessKeyId(string accessKeyId)
        {
            return _lords.SingleOrDefault(l => l.AccessKeyId == accessKeyId);
        }
    }
}
