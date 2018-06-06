using System;

namespace Experiments.PropertyParameters
{
    /// <summary>
    /// A serialization-aware wrapper for a MIME type as defined by RFC-4288
    ///
    /// https://tools.ietf.org/html/rfc4288#section-4.2
    /// </summary>
    public struct FmtType :
        IValueType
    {
        public string Name => "FMTTYPE";
        public string Value { get; }
        public bool IsEmpty => Value == null;

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

            var slashLocation = mimeType.IndexOf("/", StringComparison.Ordinal);
            if (slashLocation > 0 && slashLocation < mimeType.Length - 1)
            {
                return true;
            }

            return false;
        }

        public override string ToString() => ValueTypeUtilities.GetToString(this);
    }
}