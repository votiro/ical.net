using System.Collections.Generic;
using Experiments.ComponentProperties;

namespace Experiments.CalendarProperties
{
    /// <summary>
    /// This property defines the calendar scale used for the calendar information specified in the iCalendar object.
    ///
    /// This memo is based on the Gregorian calendar scale. The Gregorian calendar scale is assumed if this property is not specified
    /// in the iCalendar object.  It is expected that other calendar scales will be defined in other specifications or by future
    /// versions of this memo.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.7.1
    /// </summary>
    public class CalScale :
        ICalendarProperty,
        INameValueProperty
    {
        public string Name => "CALSCALE";
        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }

        /// <summary>
        /// A string representing a calendar system. Typically GREGORIAN or JULIAN.
        /// </summary>
        /// <param name="value">If left null or empty, defaults to GREGORIAN.</param>
        public CalScale(string value = "GREGORIAN", IEnumerable<string> additionalProperties = null)
        {
            Value = string.IsNullOrWhiteSpace(value)
                ? "GREGORIAN"
                : value;

            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        /// <summary>
        /// <inheritdoc cref="CalScale"/>
        /// </summary>
        /// <param name="value"></param>
        public CalScale(string value)
            : this(value, null) { }

        public CalScale()
            : this(null, null) { }

        public override string ToString() => SerializationUtilities.GetToString(this);
    }
}