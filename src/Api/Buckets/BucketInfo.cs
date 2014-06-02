using System;

namespace Api.Buckets
{
    public class BucketInfo
    {
        public string Name { get; private set; }
        public DateTime CreationDate { get; private set; }

        public BucketInfo(string name, DateTime creationDate)
        {
            Name = name;
            CreationDate = creationDate;
        }
    }
}
