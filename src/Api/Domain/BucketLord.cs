using System.Collections.Generic;
using Api.Security;
using Nancy.Security;
using Newtonsoft.Json;

namespace Api.Domain
{
    public class BucketLord : IUserIdentity
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string AccessKeyId { get; set; }
        public string SecretKey { get; set; }
        public IList<Policy> GetPoliceList { get; set; }
        [JsonIgnore]
        public string UserName { get { return DisplayName; } }
        [JsonIgnore]
        public IEnumerable<string> Claims { get { return new List<string>(); } } 
    }
}
