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
                //date: {5/24/2013 12:00:00 AM}
                //secret: "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
                //region: us-east-1
                //path: "/"
                //TODO: Fix it, this code have problem with double signing with Authorization header.
                /*var signature = S3F.ComputeSignature(credentials.Credentials.Date, bucketLord.SecretKey,
                    credentials.Credentials.Region,
                    S3F.CreateStringToSign(credentials.Credentials.Date, credentials.Credentials.Region,
                        S3F.CreateCanonicalRequest(req.Method, req.Path, string.Empty,
                            new SortedDictionary<string, string>(req.Headers.ToDictionary(p => p.Key, p => string.Join(",", p.Value))),
                            credentials.SignedHeaders.ToList(), req.Body.AsString())));*/

                var stringForSign = S3F.CreateStringToSign(credentials.Credentials.Date, credentials.Credentials.Region,
                    S3F.CreateCanonicalRequest(req.Method, req.Path, string.Empty, new SortedDictionary<string, string>(), credentials.SignedHeaders.ToList(), ""));

                var signature = S3F.ComputeSignature(credentials.Credentials.Date, bucketLord.SecretKey,
                    credentials.Credentials.Region, stringForSign);
                if (signature == credentials.Signature) return bucketLord;
            }

            return null;
        }
    }
}
