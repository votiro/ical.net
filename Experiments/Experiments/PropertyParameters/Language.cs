using Experiments.ComponentProperties;

namespace Experiments.PropertyParameters
{
    /// <summary>
    /// The RFC-5646, IETF language tag associated with a property or property parameter. Validity of language tags is NOT enforced.
    /// </summary>
    public struct Language : 
        IValueType
    {
        public string Name => "LANGUAGE";
        public string Value { get; }
        public bool IsEmpty => Value == null;

        public Language(string language)
        {
            Value = ComponentPropertiesUtilities.GetNormalizedValue(language);
        }

        public override string ToString() => ValueTypeUtilities.GetToString(this);
    }
}