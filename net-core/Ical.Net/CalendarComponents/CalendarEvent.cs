using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.Evaluation;
using Ical.Net.Utility;
using NodaDuration = NodaTime.Duration;

namespace Ical.Net.CalendarComponents
{
    /// <summary>
    /// A class that represents an RFC 5545 VEVENT component.
    /// </summary>
    /// <note>
    ///     TODO: Add support for the following properties:
    ///     <list type="bullet">
    ///         <item>Add support for the Organizer and Attendee properties</item>
    ///         <item>Add support for the Class property</item>
    ///         <item>Add support for the Geo property</item>
    ///         <item>Add support for the Priority property</item>
    ///         <item>Add support for the Related property</item>
    ///         <item>Create a TextCollection DataType for 'text' items separated by commas</item>
    ///     </list>
    /// </note>
    public class CalendarEvent :
        RecurringComponent,
        IAlarmContainer,
        IComparable<CalendarEvent>
    {
        internal const string ComponentName = "VEVENT";

        /// <summary>
        /// The start date/time of the event.
        ///
        /// If the duration has not been set, but the start/end time of the event is available, the duration is automatically determined.
        /// Likewise, if an end time and duration are available, but a start time has not been set, the start time will be extrapolated.
        /// </summary>
        public ImmutableCalDateTime Start { get; }

        /// <summary>
        /// The duration of the event.
        ///
        /// If the duration has not been set, but the start/end time of the event is available, the duration is automatically determined.
        /// Likewise, if an end time and duration are available, but a start time has not been set, the start time will be extrapolated.
        /// </summary>
        public NodaDuration Duration { get; }

        /// <summary>
        /// The duration of the event.
        ///
        /// If the duration has not been set, but the start/end time of the event is available, the duration is automatically determined.
        /// Likewise, if an end time and duration are available, but a start time has not been set, the start time will be extrapolated.
        /// </summary>
        public TimeSpan TimeSpan => Duration.ToTimeSpan();

        /// <summary>
        /// The end date/time of the event.
        ///
        /// If the duration has not been set, but the start/end time of the event is available, the duration is automatically determined.
        /// Likewise, if an end time and duration are available, but a start time has not been set, the start time will be extrapolated.
        /// </summary>
        public ImmutableCalDateTime End { get; }

        /// <summary>
        /// Returns true if the event is an all-day event.
        /// </summary>
        public bool IsAllDay => !Start.HasTime || End.Value - Start.Value >= NodaDuration.FromDays(1);

        /// <summary>
        /// The geographic location (lat/long) of the event.
        /// </summary>
        public GeographicLocation GeographicLocation { get; }
        private const string GeographicLocationKey = "GEO";

        /// <summary>
        /// The location of the event.
        /// </summary>
        public string Location { get; }
        private const string LocationKey = "LOCATION";

        /// <summary>
        /// Resources that will be used during the event.
        /// <example>Conference room #2</example>
        /// <example>Projector</example>
        /// </summary>
        public IReadOnlyList<string> Resources { get; }
        private const string ResourcesKey = "RESOURCES";

        /// <summary>
        /// The status of the event.
        /// </summary>
        public string Status { get; }
        private const string StatusKey = "STATUS";

        /// <summary>
        /// The transparency of the event.  In other words, whether or not the period of time this event occupies can contain other events
        /// (transparent), or if the time cannot be scheduled for anything else (opaque).
        /// </summary>
        public string Transparency { get; }
        private const string TransparencyKey = TransparencyType.Key;

        private readonly EventEvaluator _mEvaluator;
        public CalendarEvent()
        {
            _mEvaluator = new EventEvaluator(this);
            SetService(_mEvaluator);

            // ToDo: Add a parameter to set a parent object?
        }

        /// <summary>
        /// Use this method to determine if an event occurs on a given date.
        /// </summary>
        /// <param name="dateTime">The date to test.</param>
        /// <returns>True if the event occurs on the <paramref name="dateTime"/> provided, False otherwise.</returns>
        public virtual bool OccursOn(ImmutableCalDateTime dateTime)
        {
            return _mEvaluator.Periods.Any(p => p.StartTime.Date == dateTime.Date || // It's the start date OR
                                                (p.StartTime.Date <= dateTime.Date && // It's after the start date AND
                                                 (p.EndTime.HasTime && p.EndTime.Date >= dateTime.Date || // an end time was specified, and it's after the test date
                                                  (!p.EndTime.HasTime && p.EndTime.Date > dateTime.Date))));
        }

        public bool OccursOn(DateTime dateTime) => OccursOn(new ImmutableCalDateTime(dateTime, Start.TzId));
        public bool OccursOn(DateTimeOffset dateTimeOffset) => OccursOn(new ImmutableCalDateTime(dateTimeOffset, Start.TzId));

        /// <summary>
        /// Use this method to determine if an event begins at a given date and time.
        /// </summary>
        /// <returns>True if the event begins at the given date and time</returns>
        public virtual bool OccursAt(ImmutableCalDateTime dateTime) => _mEvaluator.Periods.Any(p => p.StartTime == dateTime);

        /// <summary>
        /// Determines whether or not the <see cref="CalendarEvent"/> is actively displayed
        /// as an upcoming or occurred event.
        /// </summary>
        /// <returns>True if the event has not been cancelled, False otherwise.</returns>
        public virtual bool IsActive => string.Equals(Status, EventStatus.Cancelled, EventStatus.Comparison);

        protected override bool EvaluationIncludesReferenceDate => true;

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);
        }

        protected override void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);

            ExtrapolateTimes();
        }

        private void ExtrapolateTimes()
        {
            //if (DurationSpan == default(TimeSpan))
            //{
            //    DurationSpan = DtEnd.Subtract(DtStart);
            //}
        }

        protected bool Equals(CalendarEvent other)
        {
            var resourcesSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            resourcesSet.UnionWith(Resources);

            var result =
                Start == other.Start
                && string.Equals(Summary, other.Summary, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Description, other.Description, StringComparison.OrdinalIgnoreCase)
                && End == other.End
                && string.Equals(Location, other.Location, StringComparison.OrdinalIgnoreCase)
                && resourcesSet.SetEquals(other.Resources)
                && string.Equals(Status, other.Status, StringComparison.Ordinal)
                && IsActive == other.IsActive
                && string.Equals(Transparency, other.Transparency, TransparencyType.Comparison)
                && EvaluationIncludesReferenceDate == other.EvaluationIncludesReferenceDate
                && Attachments.SequenceEqual(other.Attachments)
                && CollectionHelpers.Equals(ExceptionRules, other.ExceptionRules)
                && CollectionHelpers.Equals(RecurrenceRules, other.RecurrenceRules);

            if (!result)
            {
                return false;
            }

            //RDATEs and EXDATEs are all List<PeriodList>, because the spec allows for multiple declarations of collections.
            //Consequently we have to contrive a normalized representation before we can determine whether two events are equal

            var exDates = PeriodList.GetGroupedPeriods(ExceptionDates);
            var otherExDates = PeriodList.GetGroupedPeriods(other.ExceptionDates);
            if (exDates.Keys.Count != otherExDates.Keys.Count || !exDates.Keys.OrderBy(k => k).SequenceEqual(otherExDates.Keys.OrderBy(k => k)))
            {
                return false;
            }

            if (exDates.Any(exDate => !exDate.Value.OrderBy(d => d).SequenceEqual(otherExDates[exDate.Key].OrderBy(d => d))))
            {
                return false;
            }

            var rDates = PeriodList.GetGroupedPeriods(RecurrenceDates);
            var otherRDates = PeriodList.GetGroupedPeriods(other.RecurrenceDates);
            if (rDates.Keys.Count != otherRDates.Keys.Count || !rDates.Keys.OrderBy(k => k).SequenceEqual(otherRDates.Keys.OrderBy(k => k)))
            {
                return false;
            }

            if (rDates.Any(exDate => !exDate.Value.OrderBy(d => d).SequenceEqual(otherRDates[exDate.Key].OrderBy(d => d))))
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CalendarEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Start.GetHashCode();
                hashCode = (hashCode * 397) ^ (Summary?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ End.GetHashCode();
                hashCode = (hashCode * 397) ^ (Location?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Status?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ IsActive.GetHashCode();
                hashCode = (hashCode * 397) ^ Transparency?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Attachments);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Resources);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCodeForNestedCollection(ExceptionDates);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ExceptionRules);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCodeForNestedCollection(RecurrenceDates);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(RecurrenceRules);
                return hashCode;
            }
        }

        public int CompareTo(CalendarEvent other)
        {
            if (Start < other.Start)
            {
                return -1;
            }

            if (Start > other.Start)
            {
                return 1;
            }

            return 0;
        }
    }
}