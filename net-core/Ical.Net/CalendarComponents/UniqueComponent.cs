using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ical.Net.DataTypes;

namespace Ical.Net.CalendarComponents
{
    /// <summary>
    /// Represents a unique component, which has a UID, which can be used to uniquely identify the component.    
    /// </summary>
    public class UniqueComponent : CalendarComponent, IComparable<UniqueComponent>
    {
        // TODO: Add AddRelationship() public method.
        // This method will add the UID of a related component
        // to the Related_To property, along with any "RELTYPE"
        // parameter ("PARENT", "CHILD", "SIBLING", or other)
        // TODO: Add RemoveRelationship() public method.        

        public UniqueComponent()
        {
            EnsureProperties();
        }

        public UniqueComponent(string name) : base(name)
        {
            EnsureProperties();
        }

        private void EnsureProperties()
        {
            if (string.IsNullOrEmpty(Uid))
            {
                // Create a new UID for the component
                Uid = Guid.NewGuid().ToString();
            }

            // NOTE: removed setting the 'CREATED' property here since it breaks serialization.
            // See https://sourceforge.net/projects/dday-ical/forums/forum/656447/topic/3754354
        }

        public string AttendeeKey => "ATTENDEE";
        public virtual IReadOnlyList<Attendee> Attendees { get; }

        public string CommentKey => "COMMENT";
        public virtual IReadOnlyList<string> Comments { get; }

        public string DtStampKey => "DTSTAMP";
        public ImmutableCalDateTime DtStamp { get; }

        public string OrganizerKey => "ORGANIZER";
        public virtual Organizer Organizer { get; }

        public string RequestStatusesKey => "REQUEST-STATUS";
        public IReadOnlyList<string> RequestStatuses { get; }

        public string UrlKey => "URL";
        public Uri Url { get; }

        protected override void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);

            EnsureProperties();
        }

        public int CompareTo(UniqueComponent other)
            => string.Compare(Uid, other.Uid, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object obj)
        {
            if (obj is RecurringComponent && obj != this)
            {
                var r = (RecurringComponent) obj;
                if (Uid != null)
                {
                    return Uid.Equals(r.Uid);
                }
                return Uid == r.Uid;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode() => Uid?.GetHashCode() ?? base.GetHashCode();

        public string UidKey => "UID";
        public string Uid { get; }
    }
}