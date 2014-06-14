namespace Api.Configuration
{
    public interface INodeConfiguration
    {
        string Region { get; }
        string NodeEndpoint { get; }
        string RootPath { get; }
        string BucketLordsFile { get; set; }
    }
}
