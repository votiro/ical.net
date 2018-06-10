using System;
using NodaTime;

namespace Experiments.ValueTypes
{
    public struct CalPeriod
    {
        public string Name => "PERIOD";
        public string Value => ToString();
        public CalDateTime Start { get; }
        public CalDateTime End { get; }
        public Duration Duration => End - Start;
        public TimeSpan DurationSpan => Duration.ToTimeSpan();
        private readonly bool _specifiedDuration;

        public CalPeriod(CalDateTime start, CalDateTime end)
        {
            if (end <= start)
            {
                throw new ArgumentException("Start must come before End");
            }

            Start = start;
            End = end;
            _specifiedDuration = false;
        }

        public CalPeriod(CalDateTime start, Duration duration)
        {
            if (duration < Duration.Epsilon)
            {
                throw new ArgumentException("Duration should be a positive value, and probably >= 1 second");
            }

            Start = start;
            End = start + duration;
            _specifiedDuration = true;
        }

        public override string ToString()
        {
            // Start and end: 19970101T180000Z/19970102T070000Z
            // Start and duration: 19970101T180000Z/PT5H30M

            var start = Start.IsUtc
                ? Start.Value.LocalDateTime.ToString()
                : Start.Value.ToString();

            return _specifiedDuration
                ? Start.Value.LocalDateTime
        }
    }
}