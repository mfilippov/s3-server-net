using System.Linq;
using Api.Domain;
using Api.Security;
using LeviySoft.Extensions;
using Nancy;
using Nancy.Extensions;

namespace Api
{
    public class S3Request
    {
        public Request Request { get; private set; }
        public S3AuthorizationHeader S3Authorization { get; private set; }

        public S3Request(Request req)
        {
            Request = req;
            // TODO: implement querystring-based authentication
            S3Authorization = S3AuthorizationHeader.ParseHeader(req.Headers.Authorization);
        }

        public bool ValidateWith(BucketLord signer)
        {
            var canonicalRequest = S3F.CreateCanonicalRequest(
                Request.Method, 
                Request.Path, 
                string.Empty,
                Request.Headers.ToDictionary(p => p.Key, p => string.Join(",", p.Value)),
                S3Authorization.SignedHeaders.ToList(), 
                Request.Body.AsString());

            var stringToSign = S3F.CreateStringToSign(
                S3Authorization.Credentials.Date, 
                S3Authorization.Credentials.Region,
                canonicalRequest);

            var signature = S3F.ComputeSignature(
                S3Authorization.Credentials.Date, 
                signer.SecretKey,
                S3Authorization.Credentials.Region,
                stringToSign);
            return signature == S3Authorization.Signature;
        }
    }

    public static class RequestExtensions
    {
        public static S3Request ToS3Request(this Request req)
        {
            var s3Req = new S3Request(req);
            return s3Req.S3Authorization.Credentials.AccessKeyId.IsNotEmpty() ? s3Req : null;
        }
    }
}
