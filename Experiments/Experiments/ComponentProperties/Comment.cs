using System.Collections.Generic;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property specifies non-processing information intended to provide a comment to the calendar user.
    ///
    /// ANA, non-standard, alternate text representation, and language property parameters can be specified on this property.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.4
    /// </summary>
    public class Comment :
        IComponentProperty
    {
        public string Name => "COMMENT";
        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }

        public Comment(string comment, IEnumerable<string> additionalProperties)
        {
            Value = string.IsNullOrWhiteSpace(comment)
                ? null
                : comment;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}
