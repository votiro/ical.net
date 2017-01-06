using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ical.Net.Evaluation;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Interfaces.General;
using Ical.Net.Serialization.iCalendar.Serializers.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// An iCalendar representation of the <c>RRULE</c> property.
    /// </summary>
    public class RecurrencePattern : EncodableDataType, IRecurrencePattern, ICloneable
    {
        private int _interval = int.MinValue;
        private RecurrenceRestrictionType? _restrictionType;
        private RecurrenceEvaluationModeType? _evaluationMode;

        public FrequencyType Frequency { get; set; }

        public DateTime Until { get; set; } = DateTime.MinValue;

        public int Count { get; set; } = int.MinValue;

        public int Interval
        {
            get
            {
                return _interval == int.MinValue
                    ? 1
                    : _interval;
            }
            set { _interval = value; }
        }

        public List<int> BySecond { get; set; } = new List<int>(16);

        public List<int> ByMinute { get; set; } = new List<int>(16);

        public List<int> ByHour { get; set; } = new List<int>(16);

        public List<IWeekDay> ByDay { get; set; } = new List<IWeekDay>(16);

        public List<int> ByMonthDay { get; set; } = new List<int>(16);

        public List<int> ByYearDay { get; set; } = new List<int>(16);

        public List<int> ByWeekNo { get; set; } = new List<int>(16);

        public List<int> ByMonth { get; set; } = new List<int>(16);

        public List<int> BySetPosition { get; set; } = new List<int>(16);

        public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;

        public RecurrenceRestrictionType RestrictionType
        {
            get
            {
                // NOTE: Fixes bug #1924358 - Cannot evaluate Secondly patterns
                if (_restrictionType != null)
                {
                    return _restrictionType.Value;
                }
                return Calendar?.RecurrenceRestriction ?? RecurrenceRestrictionType.Default;
            }
            set { _restrictionType = value; }
        }

        public RecurrenceEvaluationModeType EvaluationMode
        {
            get
            {
                // NOTE: Fixes bug #1924358 - Cannot evaluate Secondly patterns
                if (_evaluationMode != null)
                {
                    return _evaluationMode.Value;
                }
                return Calendar?.RecurrenceEvaluationMode ?? RecurrenceEvaluationModeType.Default;
            }
            set { _evaluationMode = value; }
        }

        public RecurrencePattern()
        {
            SetService(new RecurrencePatternEvaluator(this));
        }

        public RecurrencePattern(FrequencyType frequency) : this(frequency, 1) {}

        public RecurrencePattern(FrequencyType frequency, int interval) : this()
        {
            Frequency = frequency;
            Interval = interval;
        }

        public RecurrencePattern(string value) : this()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            var serializer = new RecurrencePatternSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        protected RecurrencePattern(RecurrencePattern other) : base(other)
        {
            Frequency = other.Frequency;
            Until = other.Until;
            Count = other.Count;
            Interval = other.Interval;
            BySecond = CollectionHelpers.CloneStructs(other.BySecond).ToList();
            ByMinute = CollectionHelpers.CloneStructs(other.ByMinute).ToList();
            ByHour = CollectionHelpers.CloneStructs(other.ByHour).ToList();
            ByDay = CollectionHelpers.Clone(other.ByDay).ToList();
            ByMonthDay = CollectionHelpers.CloneStructs(other.ByMonthDay).ToList();
            ByYearDay = CollectionHelpers.CloneStructs(other.ByYearDay).ToList();
            BySetPosition = CollectionHelpers.CloneStructs(other.BySetPosition).ToList();
            FirstDayOfWeek = other.FirstDayOfWeek;
            RestrictionType = other.RestrictionType;
            EvaluationMode = other.EvaluationMode;
        }

        public override string ToString()
        {
            var serializer = new RecurrencePatternSerializer();
            return serializer.SerializeToString(this);
        }

        protected bool Equals(RecurrencePattern other)
        {
            return (Interval == other.Interval)
                && (RestrictionType == other.RestrictionType)
                && (EvaluationMode == other.EvaluationMode)
                && (Frequency == other.Frequency)
                && Until.Equals(other.Until)
                && (Count == other.Count)
                && (FirstDayOfWeek == other.FirstDayOfWeek)
                && CollectionEquals(BySecond, other.BySecond)
                && CollectionEquals(ByMinute, other.ByMinute)
                && CollectionEquals(ByHour, other.ByHour)
                && CollectionEquals(ByDay, other.ByDay)
                && CollectionEquals(ByMonthDay, other.ByMonthDay)
                && CollectionEquals(ByYearDay, other.ByYearDay)
                && CollectionEquals(ByWeekNo, other.ByWeekNo)
                && CollectionEquals(ByMonth, other.ByMonth)
                && CollectionEquals(BySetPosition, other.BySetPosition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RecurrencePattern) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Interval.GetHashCode();
                hashCode = (hashCode * 397) ^ RestrictionType.GetHashCode();
                hashCode = (hashCode * 397) ^ EvaluationMode.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Frequency;
                hashCode = (hashCode * 397) ^ Until.GetHashCode();
                hashCode = (hashCode * 397) ^ Count;
                hashCode = (hashCode * 397) ^ (int)FirstDayOfWeek;
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(BySecond);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByMinute);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByHour);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByDay);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByMonthDay);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByYearDay);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByWeekNo);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ByMonth);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(BySetPosition);
                return hashCode;
            }
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);
            if (!(obj is IRecurrencePattern))
            {
                return;
            }

            var r = (IRecurrencePattern) obj;

            Frequency = r.Frequency;
            Until = r.Until;
            Count = r.Count;
            Interval = r.Interval;
            BySecond = new List<int>(r.BySecond);
            ByMinute = new List<int>(r.ByMinute);
            ByHour = new List<int>(r.ByHour);
            ByDay = new List<IWeekDay>(r.ByDay);
            ByMonthDay = new List<int>(r.ByMonthDay);
            ByYearDay = new List<int>(r.ByYearDay);
            ByWeekNo = new List<int>(r.ByWeekNo);
            ByMonth = new List<int>(r.ByMonth);
            BySetPosition = new List<int>(r.BySetPosition);
            FirstDayOfWeek = r.FirstDayOfWeek;
            RestrictionType = r.RestrictionType;
            EvaluationMode = r.EvaluationMode;
        }

        public override object Clone()
        {
            return new RecurrencePattern(this);
            //var clone = base.Clone() as RecurrencePattern;
            //if (clone == null)
            //{
            //    return null;
            //}

            //clone.Frequency = Frequency;
            //clone.Until = Until;
            //clone.Count = Count;
            //clone.Interval = Interval;
            //clone.BySecond = CollectionHelpers.CloneStructs(BySecond).ToList();
            //clone.ByMinute = CollectionHelpers.CloneStructs(ByMinute).ToList();
            //clone.ByHour = CollectionHelpers.CloneStructs(ByHour).ToList();
            //clone.ByDay = CollectionHelpers.Clone(ByDay).ToList();
            //clone.ByMonthDay = CollectionHelpers.CloneStructs(ByMonthDay).ToList();
            //clone.ByYearDay = CollectionHelpers.CloneStructs(ByYearDay).ToList();
            //clone.BySetPosition = CollectionHelpers.CloneStructs(BySetPosition).ToList();
            //clone.FirstDayOfWeek = FirstDayOfWeek;
            //clone.RestrictionType = RestrictionType;
            //clone.EvaluationMode = EvaluationMode;
            //return clone;
        }

        private static bool CollectionEquals<T>(IEnumerable<T> c1, IEnumerable<T> c2) => c1.SequenceEqual(c2);
    }
}