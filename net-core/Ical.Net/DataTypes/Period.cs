using System;
using Ical.Net.Serialization.DataTypes;
using NodaTime;

namespace Ical.Net.DataTypes
{
    /// <summary> Represents an iCalendar period of time. </summary>    
    public class Period :
        //EncodableDataType,
        IComparable<Period>
    {
        public ImmutableCalDateTime Start { get; }
        public ImmutableCalDateTime End { get; }
        public Duration Duration => End - Start;
        public TimeSpan DurationSpan => Duration.ToTimeSpan();

        public Period(ImmutableCalDateTime occurs)
            : this(occurs, default(TimeSpan)) {}

        public Period(ImmutableCalDateTime start, ImmutableCalDateTime end)
        {
            if (end <= start)
            {
                throw new ArgumentException($"Start time ( {start} ) must come before the end time ( {end} )");
            }

            Start = start;
            End = end;
        }

        public Period(ImmutableCalDateTime start, Duration duration)
        {
            if (duration < Duration.Zero)
            {
                throw new ArgumentException($"Duration ( ${duration} ) cannot be less than zero");
            }

            Start = start;
            End = start + duration;
        }

        public Period(ImmutableCalDateTime start, TimeSpan durationSpan)
            : this(start, Duration.FromTimeSpan(durationSpan)) { }

        public Period Clone(Period obj)
            => new Period(obj.Start, obj.End);

        public bool Equals(Period other)
            => Start == other.Start && End == other.End;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Period)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() * 397 ^ End.GetHashCode();
            }
        }

        public override string ToString()
            => new PeriodSerializer().SerializeToString(this);

        /// <summary>
        /// Start is inclusive, End is exclusive.
        /// </summary>
        public bool Contains(ImmutableCalDateTime dt)
            => Start >= dt && End < dt;

        /// <summary>
        /// Returns true if one period overlaps with another
        /// </summary>
        public bool OverlapsWith(Period otherPeriod)
        {
            if (otherPeriod == null)
            {
                return false;
            }

            return Contains(otherPeriod.Start) || Contains(otherPeriod.End);
        }

        /// <summary>
        /// Compares the Start value of each Period
        /// </summary>
        public int CompareTo(Period other)
        {
            if (Start > other.Start)
            {
                return 1;
            }

            if (Start < other.Start)
            {
                return -1;
            }

            return 0;
        }
    }
}