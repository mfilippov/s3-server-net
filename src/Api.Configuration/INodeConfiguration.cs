namespace Api.Configuration
{
    public interface INodeConfiguration
    {
        string NodeEndpoint { get; }
        string RootPath { get; }
    }
}
