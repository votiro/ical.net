using System;
using System.IO;
using System.Linq;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Interfaces.General;
using Ical.Net.Serialization.iCalendar.Serializers.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// An iCalendar status code.
    /// </summary>
    public class StatusCode : EncodableDataType, IStatusCode, ICloneable
    {
        public int[] Parts { get; private set; }

        public int Primary
        {
            get
            {
                if (Parts.Length > 0)
                {
                    return Parts[0];
                }
                return 0;
            }
        }

        public int Secondary => Parts.Length > 1
            ? Parts[1]
            : 0;

        public int Tertiary => Parts.Length > 2
            ? Parts[2]
            : 0;

        public StatusCode() {}

        public StatusCode(int[] parts)
        {
            Parts = parts;
        }

        public StatusCode(string value) : this()
        {
            var serializer = new StatusCodeSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);
            var copy = obj as StatusCode;
            if (copy?.Parts == null || !copy.Parts.Any())
            {
                return;
            }

            Parts = new int[copy.Parts.Length];
            Array.Copy(copy.Parts, Parts, copy.Parts.Length);

            //base.CopyFrom(obj);
            //if (obj is IStatusCode)
            //{
            //    var sc = (IStatusCode) obj;
            //    Parts = new int[sc.Parts.Length];
            //    sc.Parts.CopyTo(Parts, 0);
            //}
        }

        public override object Clone()
        {
            var clone = base.Clone() as StatusCode;
            if (clone == null)
            {
                return null;
            }

            if (Parts == null || !Parts.Any())
            {
                return clone;
            }

            clone.Parts = new int[Parts.Length];
            Array.Copy(Parts, clone.Parts, Parts.Length);
            return clone;
        }

        public override string ToString() => new StatusCodeSerializer().SerializeToString(this);

        protected bool Equals(StatusCode other) => Parts.SequenceEqual(other.Parts);

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
            return Equals((StatusCode) obj);
        }

        public override int GetHashCode() => CollectionHelpers.GetHashCode(Parts);
    }
}