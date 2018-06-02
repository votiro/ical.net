using System;
using NodaTime;

namespace Ical.Net.DataTypes
{
    public class Occurrence :
        IComparable<Occurrence>
    {
        private readonly Period _period;
        public ImmutableCalDateTime Start => _period.Start;
        public ImmutableCalDateTime End => _period.End;
        public Duration Duration => _period.Duration;
        public TimeSpan DurationSpan => _period.DurationSpan;

        public Occurrence(ImmutableCalDateTime start, ImmutableCalDateTime end)
        {
            _period = new Period(start, end);
        }

        public Occurrence Clone(Occurrence obj)
            => new Occurrence(obj.Start, obj.End);

        public bool Equals(Occurrence other)
            => _period.Equals(other._period);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Occurrence)obj);
        }

        public override int GetHashCode()
            => _period.GetHashCode();

        public override string ToString()
            => $"{Start} ({Start.TzId}) to {End} ({End.TzId})";

        /// <summary>
        /// Start is inclusive, End is exclusive.
        /// </summary>
        public bool Contains(ImmutableCalDateTime dt)
            => Start >= dt && End < dt;

        /// <summary>
        /// Returns true if one Occurrence overlaps with another
        /// </summary>
        public bool OverlapsWith(Occurrence otherPeriod)
            => _period.OverlapsWith(otherPeriod._period);

        /// <summary>
        /// Compares the Start value of each Occurrence
        /// </summary>
        public int CompareTo(Occurrence other)
            => _period.CompareTo(other._period);
    }
}