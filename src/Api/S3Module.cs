using System;
using Nancy;

namespace Api
{
    public class S3Module : NancyModule
    {
        public S3Module()
        {
            Get["/"] = _ =>
            {
                Console.WriteLine(Request.Headers.Authorization);
                return "Welcome to S3 Server";
            };
        }
    }
}