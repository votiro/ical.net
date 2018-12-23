using System.Collections.Generic;
using Experiments.ComponentProperties;

namespace Experiments.PropertyValues
{
    /// <summary>
    /// This value type is used to identify properties that contain either a "TRUE" or "FALSE" Boolean value.
    ///
    /// These values are case-insensitive text.  No additional content value encoding(i.e., BACKSLASH character encoding, see Section 3.3.11) is defined for
    /// this value type.
    /// https://tools.ietf.org/html/rfc5545#section-3.3.2
    /// </summary>
    public struct Boolean :
        INameValueProperty
    {
        public string Name => "BOOLEAN";
        public bool BooleanValue { get; }
        public string Value => BooleanValue ? "TRUE" : "FALSE";
        public IReadOnlyList<string> Properties => null;

        public Boolean(bool value)
        {
            BooleanValue = value;
        }

        public override string ToString() => Value;
    }
}