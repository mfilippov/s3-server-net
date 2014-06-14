using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Api.Security
{
    public class Policy
    {
        public String BucketId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BucketPermission BucketPermission { get; set; }
    }
}