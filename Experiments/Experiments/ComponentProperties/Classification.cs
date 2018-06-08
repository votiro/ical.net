using System;
using System.Collections.Generic;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property defines the access classification for a calendar component.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.3
    /// </summary>
    public class Classification :
        INameValueProperty
    {
        public string Name => "CLASS";
        public string Value { get; }
        public string DefaultValue => "PUBLIC";
        public IReadOnlyList<string> Properties { get; }

        private static readonly HashSet<string> _allowedValues = new HashSet<string>(StringComparer.Ordinal) {"PUBLIC", "PRIVATE", "CONFIDENTIAL",};

        /// <summary>
        /// Allowed values are PUBLIC, PRIVATE, and CONFIDENTIAL
        /// </summary>
        public Classification(string value = null, IEnumerable<string> additionalProperties = null)
        {
            if (value == null)
            {
                Value = DefaultValue;
            }
            else if (_allowedValues.Contains(value))
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException($"Allowed {nameof(Classification)} values are {string.Join(", ", _allowedValues)}");
            }

            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}
