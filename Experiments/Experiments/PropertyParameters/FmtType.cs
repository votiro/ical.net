using System;
using System.Linq;

namespace Experiments.PropertyParameters
{
    /// <summary>
    /// A serialization-aware wrapper for a MIME type as defined by RFC-4288
    ///
    /// https://tools.ietf.org/html/rfc4288#section-4.2
    /// </summary>
    public struct FmtType
    {
        public string Name => "FMTTYPE";
        public string Value { get; }

        public FmtType(string mimeType)
        {
            Value = MimeTypeAppearsValid(mimeType)
                ? mimeType
                : null;
        }

        private static bool MimeTypeAppearsValid(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
            {
                return false;
            }

            var split = mimeType.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return split.Length == 2 && split.All(w => w.Length > 0);
        }

        public override string ToString() => Value == null ? null : $"{Name}={Value}";
    }
}