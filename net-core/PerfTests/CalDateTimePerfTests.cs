using System;
using BenchmarkDotNet.Attributes;
using Ical.Net.DataTypes;

namespace PerfTests
{
    public class CalDateTimePerfTests
    {
        private const string _aTzid = "Australia/Sydney";
        private const string _bTzid = "America/New_York";
        private static readonly DateTime _now = DateTime.Now;
        private static readonly DateTime _utcNow = DateTime.UtcNow;

        //[Benchmark]
        public IDateTime EmptyTzid() => new CalDateTime(_now);

        [Benchmark]
        public IDateTime SpecifiedTzid() => new CalDateTime(_now, _aTzid);

        [Benchmark]
        public ImmutableCalDateTime SpecifiedTzidImmutable() => new ImmutableCalDateTime(_now, _aTzid);

        [Benchmark]
        public IDateTime UtcDateTime() => new CalDateTime(_utcNow);

        [Benchmark]
        public ImmutableCalDateTime SpecifiedUtcImmutable() => new ImmutableCalDateTime(_utcNow, _aTzid);

        [Benchmark]
        public IDateTime EmptyTzidToTzid() => EmptyTzid().ToTimeZone(_bTzid);

        [Benchmark]
        public IDateTime SpecifiedTzidToDifferentTzid() => SpecifiedTzid().ToTimeZone(_bTzid);

        private static readonly ImmutableCalDateTime _specifiedImmutable = new ImmutableCalDateTime(_now, _aTzid);
        [Benchmark]
        public ImmutableCalDateTime SpecifiedTzidToDifferentTzidImmutable() => _specifiedImmutable.ToTimeZone(_bTzid);

        [Benchmark]
        public IDateTime UtcToDifferentTzid() => UtcDateTime().ToTimeZone(_bTzid);

        private static readonly ImmutableCalDateTime _utcImmutable = new ImmutableCalDateTime(_utcNow, "UTC");
        [Benchmark]
        public ImmutableCalDateTime UtcToDifferentTzidImmutable() => _utcImmutable.ToTimeZone(_bTzid);
    }
}