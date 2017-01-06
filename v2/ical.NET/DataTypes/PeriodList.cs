using System.Collections;
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
    /// An iCalendar list of recurring dates (or date exclusions)
    /// </summary>
    public class PeriodList : EncodableDataType, IPeriodList
    {
        public string TzId { get; set; }
        public int Count => Periods.Count;

        protected IList<IPeriod> Periods { get; set; } = new List<IPeriod>(64);

        public PeriodList()
        {
            SetService(new PeriodListEvaluator(this));
        }

        public PeriodList(string value) : this()
        {
            var serializer = new PeriodListSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        protected bool Equals(PeriodList other)
        {
            return string.Equals(TzId, other.TzId) && CollectionHelpers.Equals(Periods, other.Periods);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeriodList) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TzId?.GetHashCode() ?? 0) * 397) ^ CollectionHelpers.GetHashCode(Periods);
            }
        }

        public override void CopyFrom(ICopyable obj)
        {
            //base.CopyFrom(obj);
            //var list = obj as IPeriodList;
            //if (list == null)
            //{
            //    return;
            //}

            //foreach (var p in list)
            //{
            //    Add(p);
            //}
            base.CopyFrom(obj);
            var copy = obj as PeriodList;
            if (copy == null)
            {
                return;
            }

            Periods = CollectionHelpers.Clone(copy.Periods).ToList();
            TzId = copy.TzId == null
                ? null
                : string.Copy(copy.TzId);
        }

        public override object Clone()
        {
            var clone = base.Clone() as PeriodList;
            if (clone == null)
            {
                return null;
            }

            clone.Periods = CollectionHelpers.Clone(Periods).ToList();
            clone.TzId = TzId == null
                ? null
                : string.Copy(TzId);

            return clone;
        }

        public override string ToString() => new PeriodListSerializer().SerializeToString(this);

        public virtual void Add(IDateTime dt) => Periods.Add(new Period(dt));

        public IPeriod this[int index]
        {
            get { return Periods[index]; }
            set { Periods[index] = value; }
        }

        public virtual void Add(IPeriod item) => Periods.Add(item);

        public IEnumerator<IPeriod> GetEnumerator() => Periods.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Periods.GetEnumerator();
    }
}