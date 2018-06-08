using System.Collections.Generic;
using Experiments.ValueTypes;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property specifies the date and time that a calendar component ends, and can be specified in "VEVENT" or "VFREEBUSY" calendar
    /// components.
    ///
    /// Within the "VEVENT" calendar component, this property defines the date and time by which the event ends. The spec requires that
    /// the value type of this property MUST be the same as the "DTSTART" property, however this is not enforced.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.2.2
    /// </summary>
    public struct End :
        INameValueProperty
    {
        public string Name => "DTEND";
        public string Value => CalDateTime.ToString();
        public IReadOnlyList<string> Properties { get; }
        public CalDateTime CalDateTime { get; }

        public End(CalDateTime end, IEnumerable<string> additionalProperties = null)
        {
            CalDateTime = end;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public End(CalDateTime start)
            : this(start, null) { }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}