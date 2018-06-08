using System;
using Experiments.Utilities;
using NodaTime;
using NodaTime.Extensions;

namespace Experiments.ValueTypes
{
    public struct CalDateTime :
        IComparable<CalDateTime>
    {
        private DateTimeKind Kind => Value.Zone == DateTimeZone.Utc ? DateTimeKind.Utc : DateTimeKind.Local;
        public ZonedDateTime Value { get; }

        public CalDateTime(DateTime dateTime, string timeZone, bool hasTime = true)
            : this(
                zonedDateTime: DateUtil.ToZonedDateTimeLeniently(dateTime, DateUtil.GetZone(timeZone, useLocalIfNotFound: false)),
                hasTime: hasTime) { }

        public CalDateTime(DateTimeOffset dateTimeOffset, string timeZone, bool hasTime = true)
            : this(
                zonedDateTime: DateUtil.ToZonedDateTimeLeniently(dateTimeOffset, DateUtil.GetZone(timeZone, useLocalIfNotFound: false)),
                hasTime: hasTime) { }

        public CalDateTime(DateTime dateTime, bool hasTime = true)
            : this(DateUtil.ToZonedDateTimeLeniently(dateTime, DateUtil.SystemTimeZone), hasTime) { }

        private CalDateTime(
            ZonedDateTime zonedDateTime,
            bool hasTime)
        {
            Value = zonedDateTime;
            HasTime = hasTime;
        }

        public string TzId => Value.Zone.Id;
        public DateTimeZone TimeZone => Value.Zone;
        public DateTimeOffset AsDateTimeOffset => Value.ToDateTimeOffset();
        public DateTime AsUtc => Value.ToDateTimeUtc();
        public bool HasDate => true;
        public bool HasTime { get; }

        public DateTime AsSystemLocal => DateTime.SpecifyKind(Value.WithZone(DateUtil.SystemTimeZone).ToDateTimeUnspecified(), Kind);
        public DateTimeOffset AsSystemLocalOffset => Value.WithZone(DateUtil.SystemTimeZone).ToDateTimeOffset();
        public LocalDateTime AsSystemLocalDateTime => Value.LocalDateTime;
        public bool IsUtc => Value.Zone == DateTimeZone.Utc;
        public int Year => Value.Year;
        public int Month => Value.Month;
        public int Day => Value.Day;
        public int Hour => Value.Hour;
        public int Minute => Value.Minute;
        public int Second => Value.Second;
        public int Millisecond => Value.Millisecond;

        public IsoDayOfWeek IsoDayOfWeek => Value.DayOfWeek;
        public DayOfWeek DayOfWeek => Value.DayOfWeek.ToIsoDayOfWeek();
        public int DayOfYear => Value.DayOfYear;

        public DateTime LocalDateTime => DateTime.SpecifyKind(Value.ToDateTimeUnspecified(), Kind);
        public LocalDateTime Local => Value.LocalDateTime;
        public LocalDate LocalDate => Value.Date;
        public DateTime Date => DateTime.SpecifyKind(Value.Date.ToDateTimeUnspecified(), Kind);
        public LocalTime LocalTime => Value.TimeOfDay;
        public TimeSpan Time => Value.ToDateTimeUnspecified().TimeOfDay;

        public CalDateTime ToTimeZone(DateTimeZone newTimeZone)
            => new CalDateTime(Value.WithZone(newTimeZone), HasTime);

        public CalDateTime ToTimeZone(string newTimeZone)
            => new CalDateTime(Value.WithZone(DateUtil.GetZone(newTimeZone, useLocalIfNotFound: false)), HasTime);

        public static bool operator <(CalDateTime left, CalDateTime right)
            => left.Value.ToInstant() < right.Value.ToInstant();

        public static bool operator <=(CalDateTime left, CalDateTime right)
            => left.Value.ToInstant() <= right.Value.ToInstant();

        public static bool operator >(CalDateTime left, CalDateTime right)
            => left.Value.ToInstant() > right.Value.ToInstant();

        public static bool operator >=(CalDateTime left, CalDateTime right)
            => left.Value.ToInstant() >= right.Value.ToInstant();

        public bool Equals(CalDateTime other) => this == other;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CalDateTime time && Equals(time);
        }

        public static bool operator ==(CalDateTime left, CalDateTime right)
            => left.Equals(right);

        public static bool operator !=(CalDateTime left, CalDateTime right)
            => !(left == right);

        public static CalDateTime operator -(CalDateTime left, TimeSpan right)
        {
            var duration = Duration.FromTimeSpan(right);
            var newZonedValue = left.Value - duration;
            return new CalDateTime(newZonedValue, left.HasTime);
        }

        public static CalDateTime operator -(CalDateTime left, Duration duration)
        {
            var newZonedValue = left.Value - duration;
            return new CalDateTime(newZonedValue, left.HasTime);
        }

        public static Duration operator -(CalDateTime left, CalDateTime right)
        {
            var duration = left.Value - right.Value;
            return duration;
        }

        public static CalDateTime operator +(CalDateTime left, TimeSpan right)
        {
            var asDuration = Duration.FromTimeSpan(right);
            var newZonedValue = left.Value + asDuration;
            return new CalDateTime(newZonedValue, left.HasTime);
        }

        public static CalDateTime operator +(CalDateTime left, Duration duration)
        {
            var newZonedValue = left.Value + duration;
            return new CalDateTime(newZonedValue, left.HasTime);
        }

        public CalDateTime Add(TimeSpan timespan) => this + timespan;
        public CalDateTime Add(Duration duration) => this + duration;
        public CalDateTime Subtract(TimeSpan timespan) => this - timespan;
        public CalDateTime Subtract(Duration duration) => this - duration;

        public int CompareTo(CalDateTime other)
        {
            if (this == other)
            {
                return 0;
            }

            return this < other
                ? -1
                : 1;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value.GetHashCode();
                hashCode = (hashCode * 397) ^ HasTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
