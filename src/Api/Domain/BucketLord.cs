using System.Collections.Generic;
using Nancy.Security;

namespace Api.Domain
{
    public class BucketLord : IUserIdentity
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public string AccessKeyID { get; set; }
        public string SecretKey { get; set; }

        public string UserName { get { return DisplayName; } }
        public IEnumerable<string> Claims { get { return new List<string>(); } } 
    }
}
