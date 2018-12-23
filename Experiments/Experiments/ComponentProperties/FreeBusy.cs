using System;
using System.Collections.Generic;
using Experiments.ValueTypes;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// These time periods can be specified as either a start and end DATE-TIME or a start DATE-TIME and DURATION. The date and time MUST be a UTC time format.
    ///
    /// "FREEBUSY" properties within the "VFREEBUSY" calendar component SHOULD be sorted in ascending order, based on start time and then end time, with the
    /// earliest periods first.
    ///
    /// The "FREEBUSY" property can specify more than one value, separated by the COMMA character.In such cases, the "FREEBUSY" property values MUST all be of
    /// the same "FBTYPE" property parameter type (e.g., all values of a particular "FBTYPE" listed together in a single property).
    /// https://tools.ietf.org/html/rfc5545#section-3.8.2.6
    /// </summary>
    public class FreeBusy :
        INameValueProperty
    {
        public string Name => "FREEBUSY";
        public CalDateTime Start { get; }
        public CalDateTime End { get; }
        public CalDuration Duration { get; }

        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }

        public FreeBusy(CalDateTime start, CalDuration duration, IEnumerable<string> additionalProperties = null)
        {
            Start = start;
            Duration = duration;
            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public FreeBusy(CalDateTime start, CalDuration duration)
            : this(start, duration, null) { }

        public FreeBusy(CalDateTime start, CalDateTime end, IEnumerable<string> additionalProperties = null)
        {
            Start = start;
            End = end;
            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public FreeBusy(CalDateTime start, CalDateTime end)
            : this(start, end, null) { }

        public override string ToString() => SerializationUtilities.GetToString(this);
    }
}