using System;
using System.Collections.Generic;
using Ical.Net.DataTypes;

namespace Ical.Net.CalendarComponents
{
    public interface IUniqueComponent : ICalendarComponent
    {
        string Uid { get; }

        IList<Attendee> Attendees { get; }
        IList<string> Comments { get; }
        ImmutableCalDateTime DtStamp { get; }
        Organizer Organizer { get; }
        IReadOnlyList<RequestStatus> RequestStatuses { get; }
        Uri Url { get; }
    }
}