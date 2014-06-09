﻿using System;
using Nancy;
using Nancy.Security;

namespace Api.Authorization
{
    public class S3Auth
    {
        public static Func<NancyContext, IUserIdentity> Authenticate = ctx =>
        {
            if (!ctx.Request.Query.apikey.HasValue)
            {
                return null;
            }

            return null;
        };
    }
}
