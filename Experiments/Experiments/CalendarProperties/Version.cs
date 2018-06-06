namespace Experiments.CalendarProperties
{
    /// <summary>
    /// This property specifies the identifier corresponding to the highest version number or the minimum and maximum range of the
    /// iCalendar specification that is required in order to interpret the iCalendar object. This cannot be overridden.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.7.4
    /// </summary>
    public struct Version : ICalendarProperty
    {
        public string Name => "VERSION";
        public string Value => "2.0";

        public override string ToString() => $"{Name}:{Value}";
    }
}