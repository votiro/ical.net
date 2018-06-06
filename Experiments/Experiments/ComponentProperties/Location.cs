using System.Collections.Generic;
using System.Text;
using Experiments.PropertyParameters;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property defines the intended venue for the activity defined by a calendar component.
    ///
    /// Specific venues such as conference or meeting rooms may be explicitly specified using this property.An alternate representation
    /// may be specified that is a URI that points to directory information with more structured specification of the location.
    /// For example, the alternate representation may specify either an LDAP URL[RFC4516] pointing to an LDAP server entry or
    /// a CID URL[RFC2392] pointing to a MIME body part containing a Virtual-Information Card (vCard)[RFC2426] for the location.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.7
    /// </summary>
    public class Location
        : IComponentProperty
    {
        public string Name => "LOCATION";
        public string Value { get; }
        public AltRep AltRep { get; }
        public Language Language { get; }
        public IReadOnlyList<string> Properties { get; }

        public Location(string location, AltRep altRep, IEnumerable<string> additionalProperties = null)
            : this(location, additionalProperties)
        {
            if (!altRep.IsEmpty)
            {
                AltRep = altRep;
            }
        }

        public Location(string location, Language language, IEnumerable<string> additionalProperties) : this(location, additionalProperties)
        {
            if (!language.IsEmpty)
            {
                Language = language;
            }
        }

        public Location(string location, Language language, AltRep altRep, IEnumerable<string> additionalProperties) : this(location, additionalProperties)
        {
            if (!language.IsEmpty)
            {
                Language = language;
            }
            if (!altRep.IsEmpty)
            {
                AltRep = altRep;
            }
        }

        public Location(string location, IEnumerable<string> additionalProperties = null)
        {
            Value = ComponentPropertiesUtilities.GetNormalizedValue(location);
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public Location(string location)
            : this(location, null) { }

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