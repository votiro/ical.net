namespace Experiments.CalendarProperties
{
    public interface ICalendarProperty
    {
        string Name { get; }
        string Value { get; }
        string ToString();
    }
}