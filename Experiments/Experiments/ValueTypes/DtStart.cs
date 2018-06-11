using Experiments.ComponentProperties;
using Experiments.Utilities;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Experiments.ValueTypes
{
    /// <summary>
    /// This property specifies when the calendar component begins.
    ///
    /// Within the "VEVENT" calendar component, this property defines the start date and time for the event.
    /// Within the "VFREEBUSY" calendar component, this property defines the start date and time for the free or busy time information.
    /// The time MUST be specified in UTC time.
    /// Within the "STANDARD" and "DAYLIGHT" sub-components, this property defines the effective start date and time for a time zone
    /// specification.  This property is REQUIRED within each "STANDARD" and "DAYLIGHT" sub-components included in "VTIMEZONE" calendar
    /// components and MUST be specified as a date with local time without the "TZID" property parameter.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.2.4
    /// </summary>
    public struct DtStart :
        INameValueProperty,
        IDateTime
    {
        public string Name => "DTSTART";
        public string Value => ToString();
        public LocalDate Date { get; }
        public LocalTime Time { get; }
        public LocalDateTime DateTime => new LocalDateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second);
        public string TimeZoneName => TimeZone.Id;
        public IReadOnlyList<string> Properties { get; }
        public DateTimeZone TimeZone { get; }
        public bool HasTime { get; }
        private const string _tzIdKey = "TZID";
        public string TzIdKey => _tzIdKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="startTime">Will be truncated to the second</param>
        /// <param name="hasTime">If false, the time will be truncated to 00:00, and the VALUE will be DATE instead of DATE-TIME</param>
        /// <param name="timeZone"></param>
        /// <param name="additionalProperties">If a TZID property is specified, it will be ignored in favor of the timeZone. If the timeZone
        /// is null, and a TZID property is specified, it will be used</param>
        public DtStart(LocalDate startDate, LocalTime startTime, bool hasTime, DateTimeZone timeZone = null, IEnumerable<string> additionalProperties = null)
        {
            Date = startDate;
            Time = hasTime ? new LocalTime(startTime.Hour, startTime.Minute, startTime.Second) : new LocalTime(0,0);
            HasTime = hasTime;
            var properties = additionalProperties?.ToList();
            TimeZone = timeZone ?? SearchForTimeZone(properties);
            var stripTzidQuery = properties?.Where(p => p != null && !p.StartsWith(_tzIdKey, StringComparison.OrdinalIgnoreCase));
            Properties = SerializationUtilities.GetNormalizedStringCollection(stripTzidQuery);
        }

        private static DateTimeZone SearchForTimeZone(List<string> properties)
        {
            if (properties == null || properties.Count == 0)
            {
                return null;
            }

            var searchResult = properties.SingleOrDefault(p => p != null && p.StartsWith(_tzIdKey, StringComparison.OrdinalIgnoreCase));
            if (searchResult == null)
            {
                return null;
            }

            var tzid = searchResult.Substring(searchResult.IndexOf($"{_tzIdKey}=", searchResult.Length - 1, StringComparison.OrdinalIgnoreCase));
            return DateUtil.GetZone(tzid);
        }

        public DtStart(LocalDate startDate, LocalTime startTime, bool hasTime, string timeZoneName, IEnumerable<string> additionalProperties = null)
            : this(startDate, startTime, hasTime, DateUtil.GetZone(timeZoneName), additionalProperties) { }

        public DtStart(LocalDate startDate, LocalTime startTime, bool hasTime, DateTimeZone timeZone)
            : this(startDate, startTime, hasTime, timeZone, additionalProperties: null) { }

        public DtStart(LocalDate startDate, LocalTime startTime, bool hasTime, string timeZoneName)
            : this(startDate, startTime, hasTime, DateUtil.GetZone(timeZoneName), additionalProperties: null) { }

        public DtStart(LocalDate startDate, LocalTime startTime, bool hasTime)
            : this(startDate, startTime, hasTime, timeZone: null, additionalProperties: null) { }

        public DtStart(LocalDate startDate, IEnumerable<string> additionalProperties)
            : this(startDate, new LocalTime(0, 0), hasTime: false, timeZone: null, additionalProperties: additionalProperties) { }

        public DtStart(LocalDate startDate)
            : this(startDate, new LocalTime(0,0), hasTime: false, timeZone: null, additionalProperties: null) { }

        public DtStart(LocalDateTime start, bool hasTime, DateTimeZone timeZone, IEnumerable<string> additionalProperties = null)
            : this(start.Date, start.TimeOfDay, hasTime, timeZone, additionalProperties) { }

        public DtStart(LocalDateTime start, bool hasTime, string timeZoneName, IEnumerable<string> additionalProperties = null)
            : this(start.Date, start.TimeOfDay, hasTime, DateUtil.GetZone(timeZoneName), additionalProperties) { }

        public DtStart(LocalDateTime start, bool hasTime, DateTimeZone timeZone)
            : this(start.Date, start.TimeOfDay, hasTime, timeZone, additionalProperties: null) { }

        public DtStart(LocalDateTime start, bool hasTime, string timeZoneName)
            : this(start.Date, start.TimeOfDay, hasTime, DateUtil.GetZone(timeZoneName), additionalProperties: null) { }

        public DtStart(LocalDateTime start, bool hasTime)
            : this(start.Date, start.TimeOfDay, hasTime, timeZone: null, additionalProperties: null) { }

        public DtStart(DateTime start, bool hasTime, DateTimeZone timeZone, IEnumerable<string> additionalProperties = null)
            : this(LocalDateTime.FromDateTime(start), hasTime, timeZone, additionalProperties) { }

        public DtStart(DateTime start, bool hasTime, string timeZoneName, IEnumerable<string> additionalProperties = null)
            : this(LocalDateTime.FromDateTime(start), hasTime, DateUtil.GetZone(timeZoneName), additionalProperties) { }

        public DtStart(DateTime start, bool hasTime, DateTimeZone timeZone)
            : this(start, hasTime, timeZone, null) { }

        public DtStart(DateTime start, bool hasTime, string timeZoneName)
            : this(start, hasTime, DateUtil.GetZone(timeZoneName), null) { }

        public DtStart(DateTime start, bool hasTime)
            : this(start, hasTime, timeZone: null, additionalProperties: null) { }

        public void Serialize(StringBuilder builder)
        {
            SerializationUtilities.AppendDateTime(builder, this);
            builder.Append(SerializationConstants.LineBreak);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            Serialize(builder);
            return builder.ToString();
        }
    }
}