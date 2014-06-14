using System.Configuration;

namespace Api.Configuration
{
    public class NodeConfiguration : INodeConfiguration
    {
        public string Region { get; private set; }
        public string NodeEndpoint { get; private set; }
        public string RootPath { get; private set; }
        public string BucketLordsFile { get; set; }

        public NodeConfiguration()
        {
            Region = ConfigurationManager.AppSettings["Region"];
            NodeEndpoint = ConfigurationManager.AppSettings["NodeEndpoint"];
            RootPath = ConfigurationManager.AppSettings["RootPath"];
            BucketLordsFile = ConfigurationManager.AppSettings["BucketLordsFile"];
        }
    }
}
