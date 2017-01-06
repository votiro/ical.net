using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Interfaces.General;

namespace Ical.Net.DataTypes
{
    public class FreeBusyEntry : Period, IFreeBusyEntry
    {
        public virtual FreeBusyStatus Status { get; set; }

        public FreeBusyEntry()
        {
            Status = FreeBusyStatus.Busy;
        }

        public FreeBusyEntry(IPeriod period, FreeBusyStatus status)
        {
            //Sets the status associated with a given period, which requires copying the period values
            //Probably the Period object should just have a FreeBusyStatus directly?
            CopyFrom(period);
            Status = status;
        }

        protected FreeBusyEntry(FreeBusyEntry other) : base(other)
        {
            Status = other.Status;
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);

            var fb = obj as IFreeBusyEntry;
            if (fb != null)
            {
                Status = fb.Status;
            }
        }

        public override object Clone()
        {
            return new FreeBusyEntry(this);
            //var clone = base.Clone() as FreeBusyEntry;
            //if (clone == null)
            //{
            //    return new FreeBusyEntry {Status = FreeBusyStatus.Free};
            //}

            //clone.Status = Status;
            //return clone;
        }
    }
}