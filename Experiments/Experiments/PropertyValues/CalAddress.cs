using System;
using System.Collections.Generic;
using Experiments.ComponentProperties;

namespace Experiments.PropertyValues
{
    /// <summary>
    /// The value is a URI as defined by [RFC3986] or any other IANA-registered form for a URI. When used to address an Internet email transport address for a
    /// calendar user, the value MUST be a mailto URI, as defined by[RFC2368].  No additional content value encoding (i.e., BACKSLASH character encoding,
    /// see Section 3.3.11) is defined for this value type.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.3.3
    /// </summary>
    public struct CalAddress :
        INameValueProperty
    {
        public string Name => "CAL-ADDRESS";
        public Uri UriValue { get; }
        public string Value => UriValue.AbsoluteUri;
        public IReadOnlyList<string> Properties => null;

        public CalAddress(Uri uri)
        {
            UriValue = uri;
        }

        /// <summary>
        /// PERCENT-COMPLETE:39 would indicate that the VTODO is 39% complete
        /// </summary>
        public override string ToString() => UriValue.AbsoluteUri;
    }
}
