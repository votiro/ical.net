using System.Collections.Generic;
using Experiments.ValueTypes;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property defines the date and time that a to-do is expected to be completed, and can only specified in a VTODO.
    /// </summary>
    public struct Due :
        INameValueProperty
    {
        public string Name => "DUE";
        public string Value => CalDateTime.ToString();
        public IReadOnlyList<string> Properties { get; }
        public CalDateTime CalDateTime { get; }

        public Due(CalDateTime end, IEnumerable<string> additionalProperties = null)
        {
            CalDateTime = end;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Due(CalDateTime start)
            : this(start, null) { }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}