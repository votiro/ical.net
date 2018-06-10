using System;
using System.Collections.Generic;
using System.Linq;
using Experiments.ComponentProperties;
using NodaTime;

namespace Experiments.ValueTypes
{
    /// <summary>
    /// This property specifies when the calendar component begins.
    ///
    /// 
    /// </summary>
    public struct DtStart :
        INameValueProperty
    {
        public string Name => "DTSTART";
        public string Value => ToString();
        public LocalDateTime DateTime { get; }
        public Tzid Tzid { get; }
        public string TzId => Tzid.TzId;
        public IReadOnlyList<string> Properties { get; }
        public DateTimeZone TimeZone => Tzid.TimeZone;

        // Support local datetime
        // Support local date

        public DtStart(LocalDateTime start, Tzid tzid, IEnumerable<string> additionalProperties = null)
        {
            DateTime = start;
            Tzid = tzid;
            var stripTzidFromProperties = additionalProperties.Where(p => p != null && !p.StartsWith(tzid.Name, StringComparison.OrdinalIgnoreCase));
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(stripTzidFromProperties);
        }

        public DtStart(LocalDateTime start, IEnumerable<string> additionalProperties)
            : this(start, default(Tzid), additionalProperties) { }

        public DtStart(LocalDateTime start)
            : this(start, default(Tzid)) { }


        public override string ToString()
        {

            return "";
        }

        
    }
}