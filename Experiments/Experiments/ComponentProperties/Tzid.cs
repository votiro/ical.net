using System;
using Experiments.Utilities;
using NodaTime;

namespace Experiments.ComponentProperties
{
    public struct Tzid
    {
        public string Name => "TZID";
        public DateTimeZone TimeZone { get; }
        public string TzId => TimeZone.Id;
        public string Value => $"{Name}:{TzId}";
        public bool IsSystemLocal => DateUtil.SystemTimeZone == TimeZone;
        public bool HasValue => TimeZone != null;

        public Tzid(DateTimeZone timeZone)
        {
            TimeZone = timeZone;
        }

        public Tzid(string timeZoneIdentifier)
            : this(DateUtil.GetZone(timeZoneIdentifier)) { }

        public override string ToString() => HasValue ? Value : "";
    }
}