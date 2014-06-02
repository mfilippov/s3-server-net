using System;
using Api.Buckets;
using Nancy;

namespace Api
{
    public class S3Module : NancyModule
    {
        private IBucketInfoProvider _bucketInfoProvider;

        public S3Module(IBucketInfoProvider bucketInfoProvider)
        {
            _bucketInfoProvider = bucketInfoProvider;

            Get["/"] = _ =>
            {
                Console.WriteLine(Request.Headers.Authorization);
                return "Welcome to S3 Server";
            };
        }
    }
}