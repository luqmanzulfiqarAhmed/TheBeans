using System;
using Newtonsoft.Json;

namespace TheBeans.Core.Common
{
    public abstract class BaseEntity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        public DateTime CreatedAt { get; protected set; }
        public DateTime? LastModified { get; protected set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void SetModified()
        {
            LastModified = DateTime.UtcNow;
        }
    }
}

