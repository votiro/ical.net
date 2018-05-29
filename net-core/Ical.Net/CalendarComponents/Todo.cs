using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.Evaluation;
using NodaTime;
using Period = Ical.Net.DataTypes.Period;

namespace Ical.Net.CalendarComponents
{
    /// <summary>
    /// A class that represents an RFC 5545 VTODO component.
    /// </summary> 
    [DebuggerDisplay("{Summary} - {Status}")]
    public class Todo : RecurringComponent, IAlarmContainer
    {
        private readonly TodoEvaluator _mEvaluator;

        /// <summary>
        /// The date/time the todo was completed.
        /// </summary>
        public virtual ImmutableCalDateTime Completed
        {
            get => Properties.Get<ImmutableCalDateTime>("COMPLETED");
            set => Properties.Set("COMPLETED", value);
        }

        /// <summary>
        /// The start date/time of the todo item.
        /// </summary>
        public override ImmutableCalDateTime DtStart
        {
            get => base.DtStart;
            set
            {
                base.DtStart = value;
                ExtrapolateTimes();
            }
        }

        /// <summary>
        /// The due date of the todo item.
        /// </summary>
        public virtual ImmutableCalDateTime Due
        {
            get => Properties.Get<ImmutableCalDateTime>("DUE");
            set
            {
                Properties.Set("DUE", value);
                ExtrapolateTimes();
            }
        }

        /// <summary>
        /// The duration of the todo item.
        /// </summary>
        // NOTE: Duration is not supported by all systems,
        // (i.e. iPhone) and cannot co-exist with Due.
        // RFC 5545 states:
        //
        //      ; either 'due' or 'duration' may appear in
        //      ; a 'todoprop', but 'due' and 'duration'
        //      ; MUST NOT occur in the same 'todoprop'
        //
        // Therefore, Duration is not serialized, as Due
        // should always be extrapolated from the duration.
        public virtual TimeSpan Duration
        {
            get => Properties.Get<TimeSpan>("DURATION");
            set
            {
                Properties.Set("DURATION", value);
                ExtrapolateTimes();
            }
        }

        public virtual GeographicLocation GeographicLocation
        {
            get => Properties.Get<GeographicLocation>("GEO");
            set => Properties.Set("GEO", value);
        }

        public virtual string Location
        {
            get => Properties.Get<string>("LOCATION");
            set => Properties.Set("LOCATION", value);
        }

        public virtual int PercentComplete
        {
            get => Properties.Get<int>("PERCENT-COMPLETE");
            set => Properties.Set("PERCENT-COMPLETE", value);
        }

        public virtual IList<string> Resources
        {
            get => Properties.GetMany<string>("RESOURCES");
            set => Properties.Set("RESOURCES", value ?? new List<string>());
        }

        /// <summary>
        /// The status of the todo item.
        /// </summary>
        public virtual string Status
        {
            get => Properties.Get<string>(TodoStatus.Key);
            set
            {
                if (string.Equals(Status, value, TodoStatus.Comparison))
                {
                    return;
                }

                // Automatically set/unset the Completed time, once the
                // component is fully loaded (When deserializing, it shouldn't
                // automatically set the completed time just because the
                // status was changed).
                if (IsLoaded)
                {
                    var zone = DtStart.TimeZone;
                    var completedValue = string.Equals(value, TodoStatus.Completed, TodoStatus.Comparison)
                        ? new ZonedDateTime(SystemClock.Instance.GetCurrentInstant(), zone)
                        : new ZonedDateTime(Instant.MinValue, zone);

                    Completed = new ImmutableCalDateTime(completedValue);
                }

                Properties.Set(TodoStatus.Key, value);
            }
        }

        public Todo()
        {
            Name = TodoStatus.Name;

            _mEvaluator = new TodoEvaluator(this);
            SetService(_mEvaluator);
        }

        /// <summary>
        /// Use this method to determine if a todo item has been completed.
        /// This takes into account recurrence items and the previous date
        /// of completion, if any.        
        /// <note>
        /// This method evaluates the recurrence pattern for this TODO
        /// as necessary to ensure all relevant information is taken
        /// into account to give the most accurate result possible.
        /// </note>
        /// </summary>
        /// <returns>True if the todo item has been completed</returns>
        public virtual bool IsCompleted(ImmutableCalDateTime currDt)
        {
            if (!string.Equals(Status, TodoStatus.Completed, TodoStatus.Comparison))
            {
                return false;
            }

            if (Completed > currDt)
            {
                return true;
            }

            // Evaluate to the previous occurrence.
            _mEvaluator.EvaluateToPreviousOccurrence(Completed, currDt);

            return _mEvaluator.Periods.Cast<Period>().All(p => !(p.StartTime > Completed) || !(currDt >= p.StartTime));
        }

        /// <summary>
        /// Returns 'True' if the todo item is Active as of <paramref name="currDt"/>.
        /// An item is Active if it requires action of some sort.
        /// </summary>
        /// <param name="currDt">The date and time to test.</param>
        /// <returns>True if the item is Active as of <paramref name="currDt"/>, False otherwise.</returns>
        public virtual bool IsActive(ImmutableCalDateTime currDt)
            => currDt >= DtStart && !IsCompleted(currDt) && !IsCancelled;

        /// <summary>
        /// Returns True if the todo item was cancelled.
        /// </summary>
        /// <returns>True if the todo was cancelled, False otherwise.</returns>
        public virtual bool IsCancelled => string.Equals(Status, TodoStatus.Cancelled, TodoStatus.Comparison);

        protected override bool EvaluationIncludesReferenceDate => true;

        protected override void OnDeserializing(StreamingContext context)
        {
            //ToDo: a necessary evil, for now
            base.OnDeserializing(context);
        }

        private void ExtrapolateTimes()
        {
            //if (Duration == default(TimeSpan))
            //{
            //    Duration = Due.Subtract(DtStart);
            //}
        }
    }
}