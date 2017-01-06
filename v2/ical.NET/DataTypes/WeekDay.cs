using System;
using System.IO;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Interfaces.General;
using Ical.Net.Serialization.iCalendar.Serializers.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Represents an RFC 5545 "BYDAY" value.
    /// </summary>
    public class WeekDay : EncodableDataType, IWeekDay, ICloneable
    {
        public virtual int Offset { get; set; } = int.MinValue;

        public virtual DayOfWeek DayOfWeek { get; set; }

        public WeekDay()
        {
            Offset = int.MinValue;
        }

        public WeekDay(DayOfWeek day) : this()
        {
            DayOfWeek = day;
        }

        public WeekDay(DayOfWeek day, int num) : this(day)
        {
            Offset = num;
        }

        public WeekDay(DayOfWeek day, FrequencyOccurrence type) : this(day, (int) type) {}

        public WeekDay(string value)
        {
            var serializer = new WeekDaySerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        protected WeekDay(WeekDay other) : base(other)
        {
            DayOfWeek = other.DayOfWeek;
            Offset = other.Offset;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WeekDay))
            {
                return false;
            }

            var ds = (WeekDay) obj;
            return ds.Offset == Offset && ds.DayOfWeek == DayOfWeek;
        }

        public override int GetHashCode()
        {
            return Offset.GetHashCode() ^ DayOfWeek.GetHashCode();
        }

        public override void CopyFrom(ICopyable obj)
        {
            //base.CopyFrom(obj);
            //if (obj is IWeekDay)
            //{
            //    var bd = (IWeekDay) obj;
            //    Offset = bd.Offset;
            //    DayOfWeek = bd.DayOfWeek;
            //}
            base.CopyFrom(obj);
            var copy = obj as WeekDay;
            if (copy == null)
            {
                return;
            }

            DayOfWeek = copy.DayOfWeek;
            Offset = copy.Offset;
        }

        public override object Clone()
        {
            return new WeekDay(this);
            //var clone = base.Clone() as WeekDay;
            //if (clone == null)
            //{
            //    return null;
            //}

            //clone.DayOfWeek = DayOfWeek;
            //clone.Offset = Offset;
            //return clone;
        }

        public int CompareTo(object obj)
        {
            IWeekDay bd = null;
            if (obj is string)
            {
                bd = new WeekDay(obj.ToString());
            }
            else if (obj is IWeekDay)
            {
                bd = (IWeekDay) obj;
            }

            if (bd == null)
            {
                throw new ArgumentException();
            }
            var compare = DayOfWeek.CompareTo(bd.DayOfWeek);
            if (compare == 0)
            {
                compare = Offset.CompareTo(bd.Offset);
            }
            return compare;
        }
    }
}