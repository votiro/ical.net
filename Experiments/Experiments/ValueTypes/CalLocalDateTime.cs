using NodaTime;

namespace Experiments.ValueTypes
{
    public struct DtStart
    {
        public string Name => "DTSTART";

        // Support local datetime
        // Support local date

        public LocalDateTime LocalDateTimeValue { get; }
    }
}