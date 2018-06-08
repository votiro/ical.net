using System.Collections.Generic;
using Experiments.ValueTypes;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property specifies the date and time that a calendar component ends, and can be specified in "VEVENT" or "VFREEBUSY" calendar
    /// components.
    ///
    /// Within the "VEVENT" calendar component, this property defines the date and time by which the event starts.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.2.1
    /// </summary>
    public struct Start :
        INameValueProperty
    {
        public string Name => "DTSTART";
        public string Value => CalDateTime.ToString();
        public IReadOnlyList<string> Properties { get; }
        public CalDateTime CalDateTime { get; }

        public Start(CalDateTime start, IEnumerable<string> additionalProperties = null)
        {
            CalDateTime = start;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Start(CalDateTime start)
            : this(start, null) { }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}