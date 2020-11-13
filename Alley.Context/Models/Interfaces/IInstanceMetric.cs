using System;

namespace Alley.Context.Providers
{
    public interface IInstanceMetric : IComparable<IInstanceMetric>
    {
        public string Name { get;}
        public double Value { get; }
    }
}