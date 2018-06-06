using System;
using System.Collections.Generic;
using System.Text;
using Experiments.PropertyParameters;

namespace Experiments.ComponentProperties
{
    public class Resources :
        IComponentProperty
    {
        public string Name => "RESOURCES";

        /// <summary>
        /// Always null
        /// </summary>
        public string Value => null;
        public IReadOnlyList<string> Properties { get; }
        public IReadOnlyList<string> ResourceList { get; }
        public AltRep AltRep { get; }
        public Language Language { get; }
        private static readonly StringComparer _defaultComparer = StringComparer.OrdinalIgnoreCase;

        public Resources(IEnumerable<string> resources, StringComparer comparerOverride, AltRep altRep, Language language, IEnumerable<string> additionalProperties = null)
        {
            AltRep = altRep;
            Language = language;
            ResourceList = ComponentPropertiesUtilities.GetNormalizedStringCollection(resources, comparerOverride);
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Resources(IEnumerable<string> resources, StringComparer comparerOverride, IEnumerable<string> additionalProperties = null)
            : this(resources, comparerOverride, default(AltRep), default(Language), additionalProperties) { }

        public Resources(IEnumerable<string> resources, StringComparer comparerOverride)
            : this(resources, comparerOverride, default(AltRep), default(Language)) { }

        public Resources(IEnumerable<string> resources, IEnumerable<string> additionalProperties)
            : this(resources, _defaultComparer, default(AltRep), default(Language), additionalProperties) { }

        public Resources(IEnumerable<string> resources)
            : this(resources, _defaultComparer, default(AltRep), default(Language)) { }

        public Resources(IEnumerable<string> resources, StringComparer comparerOverride, AltRep altRep)
            : this(resources, comparerOverride, altRep, default(Language)) { }

        public Resources(IEnumerable<string> resources, StringComparer comparerOverride, Language language)
            : this(resources, comparerOverride, default(AltRep), language) { }

        public Resources(IEnumerable<string> resources, Language language, IEnumerable<string> additionalProperties)
            : this(resources, _defaultComparer, default(AltRep), language, additionalProperties) { }

        public Resources(IEnumerable<string> resources, Language language)
            : this(resources, _defaultComparer, language) { }

        public Resources(IEnumerable<string> resources, AltRep altRep)
            : this(resources, _defaultComparer, altRep) { }

        public override string ToString()
        {
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
            ComponentPropertiesUtilities.AppendProperties(Properties, builder);
            builder.Append($":{Value}");
            return builder.ToString();
        }
    }
}