using System;
using System.Text;
using Ical.Net.PropertyParameters;

namespace Ical.Net.ComponentProperties
{
    public class Attachment
    {
        public string Name => "ATTACH";
        public Uri Uri { get; }
        public byte[] Data { get; }
        public FmtType FormatType { get; }
        public InlineEncoding Encoding { get; }
        public string Value { get; }

        public Attachment(Uri uri)
            : this(uri, null) { }

        public Attachment(Uri uri, string formatType)
        {
            Uri = uri;
            FormatType = new FmtType(formatType);
        }

        public Attachment(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return;
            }

            Data = data;
            Encoding = new InlineEncoding(InlineEncoding.Base64);
            Value = "BINARY";
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"{Name}");

            if (Uri != null)
            {
                AppendUriString(builder);
            }
            else
            {
                AppendBinaryAttachment(builder);
            }
            builder.Append("\r\n");
            return builder.ToString();
        }

        private void AppendUriString(StringBuilder builder)
        {
            var formatType = FormatType.ToString();
            if (formatType != null)
            {
                builder.Append($";{formatType}");
            }

            builder.Append($":{Uri}");
        }

        private void AppendBinaryAttachment(StringBuilder builder)
        {
            builder.Append($";{Encoding.Name}={Encoding.ToString()}");
            builder.Append($";VALUE={Value}");
            var formatType = FormatType.ToString();
            if (formatType != null)
            {
                builder.Append($";{formatType}");
            }

            builder.Append($":{Convert.ToBase64String(Data)}");
        }
    }
}
