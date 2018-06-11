using System;
using System.Collections.Generic;
using Experiments.ComponentProperties;
using Experiments.Utilities;
using NodaTime;

namespace Experiments.ValueTypes
{
    /// <summary>
    /// This property defines the date and time that a to-do was actually completed.
    ///
    /// The property can be specified in a "VTODO" calendar component. The value MUST be specified as a date with UTC time.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.2.1
    /// </summary>
    public struct Completed :
        INameValueProperty
    {
        public string Name => "COMPLETED";
        public string Value => Timestamp.ToString();
        public IReadOnlyList<string> Properties { get; }
        public ZonedDateTime Timestamp { get; }
        public DateTime UtcDateTime => Timestamp.ToDateTimeUtc();
        public DateTimeOffset UtcDateTimeOffset => Timestamp.ToDateTimeOffset();

        /// <param name="zonedDateTime">If specified with a time zone other than UTC, the value will be converted to UTC during construction</param>
        public Completed(ZonedDateTime zonedDateTime, IEnumerable<string> additionalProperties = null)
        {
            Timestamp = zonedDateTime.Zone == DateTimeZone.Utc
                ? zonedDateTime
                : zonedDateTime.WithZone(DateTimeZone.Utc);

            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);  
        }

        /// <param name="zonedDateTime">If specified with a time zone other than UTC, the value will be converted to UTC during construction</param>
        public Completed(ZonedDateTime zonedDateTime)
            : this(zonedDateTime, null) { }

        /// <param name="dateTimeOffset">If specified with UTC offset other than 0, the value will be converted to UTC during construction</param>
        public Completed(DateTimeOffset dateTimeOffset, IEnumerable<string> additionalProperties = null)
            : this(ZonedDateTime.FromDateTimeOffset(dateTimeOffset), additionalProperties) { }

        /// <param name="dateTimeOffset">If specified with UTC offset other than 0, the value will be converted to UTC during construction</param>
        public Completed(DateTimeOffset dateTimeOffset)
            : this(dateTimeOffset, null) { }

        /// <param name="dateTime">If specified with DateTimeKind other than UTC, the local system's time zone will be used to
        /// determine what the UTC instant is.</param>
        public Completed(DateTime dateTime, IEnumerable<string> additionalProperties = null)
            : this(dateTime.Kind == DateTimeKind.Utc
                ? DateUtil.ToZonedDateTimeLeniently(dateTime, DateTimeZone.Utc)
                : DateUtil.ToZonedDateTimeLeniently(dateTime, DateUtil.SystemTimeZone).WithZone(DateTimeZone.Utc), additionalProperties)
        { }

        /// <param name="dateTime">If specified with DateTimeKind other than UTC, the local system's time zone will be used to
        /// determine what the UTC instant is.</param>
        public Completed(DateTime dateTime)
            : this(dateTime, null) { }

        public override string ToString() => SerializationUtilities.GetToString(this);
    }
}