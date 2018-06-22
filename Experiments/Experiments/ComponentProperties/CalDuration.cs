using System;
using System.Collections.Generic;
using NodaTime;
using NodaTime.Text;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property specifies a positive duration of time.
    ///
    /// In a "VEVENT" calendar component the property may be used to specify a duration of the event, instead of an explicit end DATE-TIME.
    /// In a "VTODO" calendar component the property may be used to specify a duration for the to-do, instead of an explicit due DATE-TIME.
    /// In a "VALARM" calendar component the property may be used to specify the delay period prior to repeating an alarm.
    /// When the "DURATION" property relates to a "DTSTART" property that is specified as a DATE value, then the "DURATION" property MUST
    /// be specified as a "dur-day" or "dur-week" value.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.2.5
    /// </summary>
    public struct CalDuration :
        INameValueProperty
    {
        public string Name => "DURATION";
        public string Value => PeriodPattern.NormalizingIso.Format(Period.FromTicks(Duration.BclCompatibleTicks));//Period.FromTicks(Duration.BclCompatibleTicks).ToString();
        public Duration Duration { get; }
        public TimeSpan DurationSpan => Duration.ToTimeSpan();
        public IReadOnlyList<string> Properties { get; }

        public CalDuration(Duration duration, IEnumerable<string> additionalProperties)
        {
            if (duration <= Duration.Epsilon)
            {
                throw new ArgumentException("Duration should be a positive value, and probably >= 1 second");
            }

            Duration = duration;
            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public CalDuration(Duration duration)
            : this(duration, null) { }

        public CalDuration(TimeSpan timeSpan, IEnumerable<string> additionalProperties)
            : this(Duration.FromTimeSpan(timeSpan), additionalProperties) { }

        public CalDuration(TimeSpan timeSpan)
            : this(timeSpan, null) { }

        public override string ToString() => SerializationUtilities.GetToString(this);
    }
}