using Api.Domain;
using Nancy;

namespace Api.Security
{
    public interface IFaceControlService
    {
        BucketLord CheckAuth(Request req);
    }
}
