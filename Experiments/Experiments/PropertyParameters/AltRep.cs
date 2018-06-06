using System;

namespace Experiments.PropertyParameters
{
    /// <summary>
    /// Specified an alternate text representation for the property value.
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
        /// ALTREP="http://example.com/foo/bar"
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value == null ? null : $"{Name}:\"{Value}\"";
    }
}