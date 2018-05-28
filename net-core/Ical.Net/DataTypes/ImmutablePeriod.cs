using System;
using Ical.Net.Serialization.DataTypes;
using NodaTime;

namespace Ical.Net.DataTypes
{
    /// <summary> Represents an iCalendar period of time. </summary>    
    public struct ImmutablePeriod :
        IComparable<ImmutablePeriod>
        //, EncodableDataType
    {
        public ImmutableCalDateTime Start { get; }
        public DateTime StartDateTime => Start.LocalDateTime;
        public LocalDateTime StartLocal => Start.Local;
        public DateTimeOffset StartDateTimeOffset => Start.AsDateTimeOffset;

        public ImmutableCalDateTime End { get; }
        public DateTime EndDateTime => Start.LocalDateTime;
        public LocalDateTime EndLocal => Start.Local;
        public DateTimeOffset EndDateTimeOffset => Start.AsDateTimeOffset;

        public Duration Duration { get; }
        public TimeSpan DurationSpan => Duration.ToTimeSpan();

        public ImmutablePeriod(ImmutableCalDateTime start, ImmutableCalDateTime end)
        {
            if (end <= start)
            {
                throw new ArgumentException($"Start time ( {start} ) must come before the end time ( {end} )");
            }

            Start = start;
            End = end;
            Duration = end.Value - start.Value;
        }

        public ImmutablePeriod(ImmutableCalDateTime start, Duration duration)
        {
            if (duration == Duration.Zero)
            {
                throw new ArgumentException("Duration must have a value greater than zero");
            }

            Start = start;
            End = Start + duration;
            Duration = duration;
        }

        public ImmutablePeriod(ImmutableCalDateTime start, TimeSpan duration)
            : this(start, Duration.FromTimeSpan(duration)) { }

        public bool Equals(ImmutablePeriod other)
            => Start == other.Start && End == other.End;

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj.GetType() == GetType() && Equals((ImmutablePeriod)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Start.GetHashCode();
                hashCode = (hashCode * 397) ^ End.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            var periodSerializer = new PeriodSerializer();
            return periodSerializer.SerializeToString(this);
        }

        public bool Contains(ImmutableCalDateTime dt)
            => dt >= Start && dt < End; // Start is inclusive, End is exclusive

        public bool CollidesWith(ImmutablePeriod period)
            => Contains(period.Start) || Contains(period.End);

        public int CompareTo(ImmutablePeriod other)
        {
            if (Start == other.Start)
            {
                return 0;
            }
            if (Start < other.Start)
            {
                return -1;
            }
            if (Start > other.Start)
            {
                return 1;
            }
            throw new Exception("An error occurred while comparing two Periods.");
        }
    }
}