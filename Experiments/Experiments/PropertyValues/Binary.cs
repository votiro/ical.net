using System;
using System.Collections.Generic;
using Experiments.ComponentProperties;
using Experiments.Utilities;

namespace Experiments.PropertyValues
{
    /// <summary>
    /// This value type is used to identify properties that contain a character encoding of inline binary data.For example, an inline attachment of a document
    /// might be included in an iCalendar object.
    ///
    /// Property values with this value type MUST also include the inline encoding parameter sequence of ";ENCODING=BASE64". That is, all inline binary data
    /// MUST first be character encoded using the "BASE64" encoding method defined in [RFC2045]. No additional content value encoding (i.e., BACKSLASH
    /// character encoding, see Section 3.3.11) is defined for this value type.
    /// https://tools.ietf.org/html/rfc5545#section-3.3.1
    /// </summary>
    public struct Binary :
        INameValueProperty
    {
        public string Name => "BINARY";
        public string Value { get; }
        public IReadOnlyList<string> Properties => null;

        public Binary(byte[] content)
        {
            Value = Convert.ToBase64String(content);
        }

        public Binary(string base64String)
        {
            if (!base64String.IsBase64())
            {
                throw new ArgumentException("Not a valid base64 string", nameof(base64String));
            }

            Value = base64String;
        }

        public override string ToString() => Value == null ? "" : $"{Name}:{Value}";
    }
}