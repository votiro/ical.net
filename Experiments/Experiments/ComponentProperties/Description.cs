using System.Collections.Generic;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property provides a more complete description of the calendar component than that provided by the "SUMMARY" property.
    ///
    /// This property is used in the "VEVENT" and "VTODO" to capture lengthy textual descriptions associated with the activity.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.5
    /// </summary>
    public class Description :
        IComponentProperty
    {
        public string Name => "DESCRIPTION";
        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }

        public Description(string description, IEnumerable<string> additionalProperties)
        {
            Value = ComponentPropertiesUtilities.GetNormalizedValue(description);
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Description(string description)
            : this(description, null) { }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}
