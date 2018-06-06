using System;
using System.Collections.Generic;
using System.Text;
using Experiments.PropertyParameters;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property provides the capability to associate a document object with a calendar component.
    ///
    /// This property is used in "VEVENT", "VTODO", and "VJOURNAL" calendar components to associate a resource(e.g., document) with the calendar component.
    /// This property is used in "VALARM" calendar components to specify an audio sound resource or an email message attachment.This property can be
    /// specified as a URI pointing to a resource or as inline binary encoded content.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.1
    /// </summary>
    public class Attachment
        : IComponentProperty
    {
        public string Name => "ATTACH";
        public Uri Uri { get; }
        public byte[] Data { get; }
        public FmtType FormatType { get; }
        public InlineEncoding Encoding { get; }
        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }

        public Attachment(Uri uri)
            : this(uri, formatType: null) { }

        public Attachment(Uri uri, string formatType, IEnumerable<string> additionalProperties = null)
            : this(uri, new FmtType(formatType), additionalProperties) { }

        public Attachment(Uri uri, FmtType formatType, IEnumerable<string> additionalProperties = null)
        {
            Uri = uri;
            FormatType = formatType;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Attachment(byte[] data, IEnumerable<string> additionalProperties = null)
        {
            if (data == null || data.Length == 0)
            {
                return;
            }

            Data = data;
            Encoding = new InlineEncoding(InlineEncoding.Base64);
            Value = "BINARY";
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"{Name}");

            // FormatType, then additional properties, then URI or binary attachment
            var formatType = FormatType.ToString();
            if (formatType != null)
            {
                builder.Append($";{formatType}");
            }

            ComponentPropertiesUtilities.AppendProperties(Properties, builder);

            if (Uri != null)
            {
                builder.Append($":{Uri}");
            }
            else
            {
                AppendBinaryAttachment(builder);
            }
            builder.Append("\r\n");
            return builder.ToString();
        }

        private void AppendBinaryAttachment(StringBuilder builder)
        {
            builder.Append($";{Encoding.Name}={Encoding.ToString()}");
            builder.Append($";VALUE={Value}");
            builder.Append($":{Convert.ToBase64String(Data)}");
        }
    }
}
