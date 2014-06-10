using System;
using System.Globalization;
using System.Linq;
using Api.Domain;
using LeviySoft.Extensions;
using Nancy;

namespace Api.Security
{
    public class FaceControlService : IFaceControlService
    {
        private BucketLordTemple _lordTemple;

        public FaceControlService(BucketLordTemple lordTemple)
        {
            _lordTemple = lordTemple;
        }

        public BucketLord CheckAuth(Request req)
        {
            // TODO: implement querystring-based authentication
            var authString = req.Headers.Authorization;
            var authStringComponents = authString.Split(',');

            if (authStringComponents.Count() != 4) return null;

            var authParams =
                authStringComponents.Where(c => c.Contains("="))
                    .Select(c => c.Split('='))
                    .ToDictionary(c => c[0], c => c[1]);

            if (authParams.ContainsKeys("Credential", "SignedHeaders", "Signature"))
            {
                var credentials = authParams["Credential"].Split('/');
                var accessKeyId = credentials[0];

                var bucketLord = _lordTemple.FindLordByAccessKeyId(accessKeyId);
                if (bucketLord == null) return null;

                //TODO: complete
                var sigDate = DateTime.ParseExact(credentials[1], "yyyyMMdd", CultureInfo.InvariantCulture);
                var signedHeaders = authParams["SignedHeaders"].Split(';');

            }

            return null;
        }
    }
}
