using System;

namespace Experiments.PropertyParameters
{
    /// <summary>
    /// This parameter specifies a URI that points to an alternate representation for a textual property value.
    ///
    /// A property specifying this parameter MUST also include a value that reflects the default representation of the text value. The URI parameter value MUST
    /// be specified in a quoted-string.
    ///
    /// Note: While there is no restriction imposed on the URI schemes allowed for this parameter, Content Identifier (CID)[RFC2392], HTTP[RFC2616],
    /// and HTTPS [RFC2818] are the URI schemes most commonly used by current implementations.
    /// https://tools.ietf.org/html/rfc5545#section-3.2.1
    /// </summary>
    public struct AltRep :
        IValueType
    {
        public string Name => "ALTREP";
        public string Value => Uri?.ToString();
        public Uri Uri { get; }
        public bool IsEmpty => Uri == null;

        public AltRep(Uri value)
        {
            Uri = value;
        }

        /// <summary>
        /// ALTREP="CID:part3.msg.970415T083000@example.com"
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value == null ? "" : $"{Name}:\"{Value}\"";

        public static void VerifyAltRep(AltRep altRep, string parameter, string parameterName)
        {
            if (altRep.IsEmpty)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException("Alternative representations MUST include a default string parameter", parameterName);
            }
        }
    }
}