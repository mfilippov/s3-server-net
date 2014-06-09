using System.Configuration;

namespace Api.Configuration
{
    public class NodeConfiguration : INodeConfiguration
    {
        public string NodeEndpoint { get; private set; }
        public string RootPath { get; private set; }

        public NodeConfiguration()
        {
            NodeEndpoint = ConfigurationManager.AppSettings["NodeEndpoint"];
            RootPath = ConfigurationManager.AppSettings["RootPath"];
        }
    }
}
