using System;
using System.Diagnostics;
using System.IO;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Interfaces.General;
using Ical.Net.Serialization.iCalendar.Serializers.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// A class that represents the organizer of an event/todo/journal.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class Organizer : EncodableDataType, IOrganizer
    {
        public virtual Uri SentBy
        {
            get
            {
                if (!Parameters.ContainsKey("SENT-BY"))
                {
                    SentBy = null;
                }
                var value = Parameters.Get("SENT-BY");
                return value == null
                    ? null
                    : new Uri(value);
            }
            set
            {
                if (value != null)
                {
                    Parameters.Set("SENT-BY", value.OriginalString);
                }
                else
                {
                    Parameters.Set("SENT-BY", (string) null);
                }
            }
        }

        public virtual string CommonName
        {
            get { return Parameters.Get("CN"); }
            set { Parameters.Set("CN", value); }
        }

        public virtual Uri DirectoryEntry
        {
            get
            {
                if (!Parameters.ContainsKey("DIR"))
                {
                    DirectoryEntry = null;
                }
                var value = Parameters.Get("DIR");
                return value == null
                    ? null
                    : new Uri(value);
            }
            set
            {
                if (value != null)
                {
                    Parameters.Set("DIR", value.OriginalString);
                }
                else
                {
                    Parameters.Set("DIR", (string) null);
                }
            }
        }

        public virtual Uri Value { get; set; }

        public Organizer() {}

        public Organizer(string value) : this()
        {
            var serializer = new OrganizerSerializer();
            using (var reader = new StringReader(value))
            {
                CopyFrom(serializer.Deserialize(reader) as ICopyable);
            }
        }

        protected Organizer(Organizer other) : base(other)
        {
            CommonName = other.CommonName == null
                ? null
                : string.Copy(other.CommonName);

            DirectoryEntry = other.DirectoryEntry == null
                ? null
                : new Uri(other.DirectoryEntry.OriginalString);

            SentBy = other.SentBy == null
                ? null
                : new Uri(other.SentBy.OriginalString);

            Value = other.Value == null
                ? null
                : new Uri(other.Value.OriginalString);
        }

        protected bool Equals(Organizer other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Organizer) obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);
            var copy = obj as Organizer;
            if (copy == null)
            {
                return;
            }

            CommonName = copy.CommonName == null
                ? null
                : string.Copy(copy.CommonName);

            DirectoryEntry = string.IsNullOrEmpty(copy.DirectoryEntry?.OriginalString)
                ? null
                : new Uri(copy.DirectoryEntry.OriginalString);

            SentBy = copy.SentBy == null
                ? null
                : new Uri(copy.SentBy.OriginalString);

            Value = copy.Value == null
                ? null
                : new Uri(copy.Value.OriginalString);
        }

        public override object Clone()
        {
            return new Organizer(this);
            //var clone = base.Clone() as Organizer;
            //if (clone == null)
            //{
            //    return new Organizer();
            //}

            //clone.CommonName = CommonName == null
            //    ? null
            //    : string.Copy(CommonName);

            //clone.DirectoryEntry = DirectoryEntry == null
            //    ? null
            //    : new Uri(DirectoryEntry.OriginalString);

            //clone.SentBy = SentBy == null
            //    ? null
            //    : new Uri(SentBy.OriginalString);

            //clone.Value = Value == null
            //    ? null
            //    : new Uri(Value.OriginalString);

            //return clone;
        }
    }
}