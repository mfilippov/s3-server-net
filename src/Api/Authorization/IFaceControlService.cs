using Api.Domain;
using Nancy;

namespace Api.Authorization
{
    public interface IFaceControlService
    {
        BucketLord CheckAuth(Request req);
    }
}
