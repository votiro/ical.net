using System;
using System.Collections.Generic;
using System.Text;
using Ical.Net.DataTypes;
using Ical.Net.Utility;
using NodaTime;

namespace Ical.Net
{
    public class ImmutableCalDateTime //: IDateTime
    {
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
            => !left.Equals(right);

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

        protected bool Equals(ImmutableCalDateTime other)
        {
            return _hasTime == other._hasTime
               && string.Equals(_timeZone, other._timeZone, StringComparison.OrdinalIgnoreCase)
               && _zonedValue.Equals(other._zonedValue)
               && _hasTime == other._hasTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((ImmutableCalDateTime)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _timeZone?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ _zonedValue.GetHashCode();
                hashCode = (hashCode * 397) ^ _hasTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
