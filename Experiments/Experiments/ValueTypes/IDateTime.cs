using Experiments.ComponentProperties;
using NodaTime;

namespace Experiments.ValueTypes
{
    public interface IDateTime :
        INameValueProperty
    {
        LocalDate Date { get; }
        LocalTime Time { get; }
        string TimeZoneName { get; }
        DateTimeZone TimeZone { get; }
        bool HasTime { get; }
        string TzIdKey { get; }
    }
}