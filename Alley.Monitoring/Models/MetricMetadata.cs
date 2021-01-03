using System;

namespace Alley.Monitoring.Models
{
    public class MetricMetadata : IEquatable<MetricMetadata>
    {
        public string Instance { get; set; }
        public string Job { get; set; }

        public bool Equals(MetricMetadata other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Instance == other.Instance && Job == other.Job;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MetricMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Instance, Job);
        }
    }
}