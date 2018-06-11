using System.Collections.Generic;
using System.Text;
using Experiments.PropertyParameters;
using Experiments.Utilities;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property defines a short summary or subject for the calendar component.
    ///
    /// This property is used in the "VEVENT", "VTODO", and "VJOURNAL" calendar components to capture a short, one-line summary about the activity or
    /// journal entry. This property is used in the "VALARM" calendar component to capture the subject of an EMAIL category of alarm.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.12
    /// </summary>
    public class Summary :
        INameValueProperty
    {
        public string Name => "SUMMARY";
        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }
        public AltRep AltRep { get; }
        public Language Language { get; }

        public Summary(string summary, AltRep altRep, Language language, IEnumerable<string> additionalProperties)
        {
            if (string.IsNullOrWhiteSpace(summary))
            {
                return;
            }

            AltRep = altRep;
            Language = language;
            Value = summary;
            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Summary(string summary, Language language)
            : this(summary, default(AltRep), language, null) { }

        public Summary(string summary, Language language, IEnumerable<string> addtionalProperties)
            : this(summary, default(AltRep), language, addtionalProperties) { }
        
        public Summary(string summary, AltRep altRep, IEnumerable<string> additionalProperties):
            this(summary, altRep, default(Language), additionalProperties) { }

        public Summary(string summary, AltRep altRep)
            : this(summary, altRep, default(Language), null) { }

        public Summary(string summary, IEnumerable<string> additionalProperties)
            : this(summary, default(AltRep), default(Language), additionalProperties) { }

        public Summary(string summary)
            : this(summary, default(AltRep), default(Language), null) { }

        public override string ToString()
        {
            if (Value == null)
            {
                return "";
            }

            var builder = new StringBuilder();
            builder.Append($"{Name}");

            // altrep then language then other properties
            if (!AltRep.IsEmpty)
            {
                builder.Append($";{AltRep.ToString()}");
            }

            if (!Language.IsEmpty)
            {
                builder.Append($";{Language.ToString()}");
            }
            SerializationUtilities.AppendProperties(Properties, builder);
            builder.Append($":{Value}{SerializationConstants.LineBreak}");
            return builder.ToString();
        }
    }
}
