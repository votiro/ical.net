namespace Experiments.CalendarProperties
{
    /// <summary>
    /// This property specifies the identifier for the product that created the iCalendar object.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.7.3
    /// </summary>
    public class ProdId : ICalendarProperty
    {
        public string Name => "PRODID";
        public string Value { get; }

        /// <summary>
        /// If overridden, you SHOULD ensure that this is a globally unique identifier; using some technique such as an FPI value,
        /// as defined in [ISO.9070.1991]
        /// </summary>
        public ProdId(string value = null)
        {
            Value = string.IsNullOrWhiteSpace(value)
                ? "-//github.com/rianjs/ical.net//NONSGML ical.net 4.0//EN"
                : value;
        }

        public override string ToString() => $"{Name}:{Value}";
    }
}