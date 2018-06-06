namespace Experiments.CalendarProperties
{
    /// <summary>
    /// This property defines the iCalendar object method associated with the calendar object.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.7.2
    /// </summary>
    public struct Method : ICalendarProperty
    {
        public string Name => "METHOD";
        public string Value { get; }

        /// <summary>
        /// The following is a hypothetical example of this property to convey that the iCalendar object is a scheduling request:
        /// METHOD:REQUEST
        /// </summary>
        public Method(string value = null)
        {
            Value = string.IsNullOrWhiteSpace(value)
                ? null
                : value;
        }

        public override string ToString()
        {
            return Value == null
                ? null
                : $"{Name}:{Value}";
        }
    }
}