using System.Configuration;

namespace Api.Configuration
{
    public class NodeConfiguration : INodeConfiguration
    {
        public string NodeUri { get; private set; }
        public string RootPath { get; private set; }

        public NodeConfiguration()
        {
            NodeUri = ConfigurationManager.AppSettings["NodeUri"];
            RootPath = ConfigurationManager.AppSettings["RootPath"];
        }
    }
}
