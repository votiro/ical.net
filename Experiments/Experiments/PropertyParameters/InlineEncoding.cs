namespace Experiments.PropertyParameters
{
    /// <summary>
    /// This property parameter identifies the inline encoding used in a property value.The default encoding is "8BIT", corresponding
    /// to a property value consisting of text. The "BASE64" encoding type corresponds to a property value encoded using the "BASE64"
    /// encoding defined in [RFC2045].
    ///
    /// This property is typically optional.
    /// </summary>
    /// <remarks>This property is typically optional, except in binary attachments</remarks>
    public struct InlineEncoding :
        IValueType
    {
        public string Name => "ENCODING";
        public string Value { get; }
        public bool IsEmpty => Value == null;

        public InlineEncoding(string encoding = null)
        {
            Value = string.IsNullOrWhiteSpace(encoding)
                ? null
                : encoding;
        }

        public override string ToString() => Value == null ? null : $"{Name}={Value}";

        public static string Base64 => "BASE64";
        public static string Default => EightBit;
        public static string EightBit => "8BIT";
    }
}