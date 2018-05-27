using System;
using Ical.Net.Utility;
using NodaTime;
using NodaTime.Extensions;

namespace Ical.Net.DataTypes
{
    public struct ImmutableCalDateTime :
        IComparable<ImmutableCalDateTime>
        //, IDateTime
    {
        private static readonly DateTimeZone _systemTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
        private DateTimeKind Kind => _zonedValue.Zone == DateTimeZone.Utc ? DateTimeKind.Utc : DateTimeKind.Local;
        private readonly ZonedDateTime _zonedValue;
        private readonly bool _hasTime;

        public ImmutableCalDateTime(DateTime dateTime, string timeZone, bool hasTime = true)
            : this(
                zonedDateTime: DateUtil.ToZonedDateTimeLeniently(dateTime, DateUtil.GetZone(timeZone, useLocalIfNotFound: false)),
                hasTime: hasTime) { }

        public ImmutableCalDateTime(DateTimeOffset dateTimeOffset, string timeZone, bool hasTime = true)
            : this(
                zonedDateTime: DateUtil.ToZonedDateTimeLeniently(dateTimeOffset, DateUtil.GetZone(timeZone, useLocalIfNotFound: false)),
                hasTime: hasTime) { }

        public ImmutableCalDateTime(ZonedDateTime zonedDateTime, bool hasTime = true)
        {
            if (zonedDateTime == null)
            {
                throw new ArgumentNullException(nameof(zonedDateTime));
            }
            _zonedValue = zonedDateTime;
            _hasTime = hasTime;
        }

        public string TzId => _zonedValue.Zone.Id;
        public DateTimeOffset AsDateTimeOffset => _zonedValue.ToDateTimeOffset();
        public DateTime AsUtc => _zonedValue.ToDateTimeUtc();
        public bool HasDate => true;
        public bool HasTime => _hasTime;
        public DateTime AsSystemLocal => DateTime.SpecifyKind(_zonedValue.WithZone(_systemTimeZone).ToDateTimeUnspecified(), Kind);
        public DateTimeOffset AsSystemLocalOffset => _zonedValue.WithZone(_systemTimeZone).ToDateTimeOffset();
        public LocalDateTime AsSystemLocalDateTime => _zonedValue.LocalDateTime;
        public bool IsUtc => _zonedValue.Zone == DateTimeZone.Utc;
        public int Year => _zonedValue.Year;
        public int Month => _zonedValue.Month;
        public int Day => _zonedValue.Day;
        public int Hour => _zonedValue.Hour;
        public int Minute => _zonedValue.Minute;
        public int Second => _zonedValue.Second;
        public int Millisecond => _zonedValue.Millisecond;

        public IsoDayOfWeek IsoDayOfWeek => _zonedValue.DayOfWeek;
        public DayOfWeek DayOfWeek => _zonedValue.DayOfWeek.ToIsoDayOfWeek();
        public int DayOfYear => _zonedValue.DayOfYear;

        public LocalDate LocalDate => _zonedValue.Date;
        public DateTime Date => DateTime.SpecifyKind(_zonedValue.Date.ToDateTimeUnspecified(), Kind);
        public LocalTime LocalTime => _zonedValue.TimeOfDay;
        public TimeSpan Time => _zonedValue.ToDateTimeUnspecified().TimeOfDay;

        public ImmutableCalDateTime ToTimeZone(DateTimeZone newTimeZone)
            => new ImmutableCalDateTime(_zonedValue.WithZone(newTimeZone), _hasTime);

        public ImmutableCalDateTime ToTimeZone(string newTimeZone)
            => new ImmutableCalDateTime(_zonedValue.WithZone(DateUtil.GetZone(newTimeZone, useLocalIfNotFound: false)), _hasTime);

        public static bool operator <(ImmutableCalDateTime left, ImmutableCalDateTime right)
            => left._zonedValue.ToInstant() < right._zonedValue.ToInstant();

        public static bool operator <=(ImmutableCalDateTime left, ImmutableCalDateTime right)
            => left._zonedValue.ToInstant() <= right._zonedValue.ToInstant();

        public static bool operator >(ImmutableCalDateTime left, ImmutableCalDateTime right)
            => left._zonedValue.ToInstant() > right._zonedValue.ToInstant();

        public static bool operator >=(ImmutableCalDateTime left, ImmutableCalDateTime right)
            => left._zonedValue.ToInstant() >= right._zonedValue.ToInstant();

        public static bool operator ==(ImmutableCalDateTime left, ImmutableCalDateTime right)
            => left.Equals(right);

        public static bool operator !=(ImmutableCalDateTime left, ImmutableCalDateTime right)
            => !(left == right);

        public static ImmutableCalDateTime operator -(ImmutableCalDateTime left, TimeSpan right)
        {
            var duration = Duration.FromTimeSpan(right);
            var newZonedValue = left._zonedValue - duration;
            return new ImmutableCalDateTime(newZonedValue, left.HasTime);
        }

        public static ImmutableCalDateTime operator -(ImmutableCalDateTime left, Duration duration)
        {
            var newZonedValue = left._zonedValue - duration;
            return new ImmutableCalDateTime(newZonedValue, left.HasTime);
        }

        public static ImmutableCalDateTime operator +(ImmutableCalDateTime left, TimeSpan right)
        {
            var asDuration = Duration.FromTimeSpan(right);
            var newZonedValue = left._zonedValue + asDuration;
            return new ImmutableCalDateTime(newZonedValue, left.HasTime);
        }

        public static ImmutableCalDateTime operator +(ImmutableCalDateTime left, Duration duration)
        {
            var newZonedValue = left._zonedValue + duration;
            return new ImmutableCalDateTime(newZonedValue, left.HasTime);
        }

        public ImmutableCalDateTime Add(TimeSpan timespan) => this + timespan;
        public ImmutableCalDateTime Add(Duration duration) => this + duration;
        public ImmutableCalDateTime Subtract(TimeSpan timespan) => this - timespan;
        public ImmutableCalDateTime Subtract(Duration duration) => this - duration;

        public int CompareTo(ImmutableCalDateTime other)
        {
            if (this == other)
            {
                return 0;
            }

            return this < other
                ? -1
                : 1;
        }

        public bool Equals(ImmutableCalDateTime other) => _hasTime == other._hasTime && _zonedValue.Equals(other._zonedValue);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _zonedValue.GetHashCode();
                hashCode = (hashCode * 397) ^ _hasTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
