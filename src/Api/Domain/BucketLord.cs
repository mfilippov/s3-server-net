using System.Collections.Generic;
using System.Xml.Serialization;
using Nancy.Security;

namespace Api.Domain
{
    public class BucketLord : IUserIdentity
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public string AccessKeyID { get; set; }
        public string SecretKey { get; set; }

        [XmlIgnore]
        public string UserName { get { return DisplayName; } }

        [XmlIgnore]
        public IEnumerable<string> Claims { get { return new List<string>(); } } 
    }
}
