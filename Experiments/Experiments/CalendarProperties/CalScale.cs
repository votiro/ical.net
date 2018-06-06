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
    public struct CalScale : ICalendarProperty
    {
        public string Name => "CALSCALE";
        public string Value { get; }

        /// <summary>
        /// A string representing a calendar system. Typically GREGORIAN or JULIAN.
        /// </summary>
        /// <param name="value">If left null or empty, defaults to GREGORIAN.</param>
        public CalScale(string value = null)
        {
            Value = string.IsNullOrWhiteSpace(value)
                ? "GREGORIAN"
                : value;
        }

        public override string ToString() => $"{Name}:{Value}";
    }
}