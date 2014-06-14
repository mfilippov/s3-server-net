using System.Collections.Generic;
using System.Linq;
using Api.Domain;
using LeviySoft.Extensions;
using Nancy;
using Nancy.Extensions;

namespace Api.Security
{
    public class FaceControlService : IFaceControlService
    {
        private readonly IBucketLordTemple _lordTemple;

        public FaceControlService(IBucketLordTemple lordTemple)
        {
            _lordTemple = lordTemple;
        }

        public BucketLord CheckAuth(Request req)
        {
            // TODO: implement querystring-based authentication
            var credentials = S3AuthorizationHeader.ParseHeader(req.Headers.Authorization);

            if (credentials.Credentials.AccessKeyId.IsNotEmpty())
            {
                var bucketLord = _lordTemple.FindLordByAccessKeyId(credentials.Credentials.AccessKeyId);
                if (bucketLord == null) return null;

                var signature = S3F.ComputeSignature(credentials.Credentials.Date, bucketLord.SecretKey,
                    credentials.Credentials.Region,
                    S3F.CreateStringToSign(credentials.Credentials.Date, credentials.Credentials.Region,
                        S3F.CreateCanonicalRequest(req.Method, req.Path, string.Empty,
                            new SortedDictionary<string, string>(req.Headers.ToDictionary(p => p.Key, p => string.Join(",", p.Value))),
                            credentials.SignedHeaders.ToList(), req.Body.AsString())));
                if (signature == credentials.Signature) return bucketLord;
            }

            return null;
        }
    }
}
