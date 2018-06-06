using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Experiments.ComponentProperties
{
    public static class ComponentPropertiesUtilities
    {
        /// <summary>
        /// Returns null if the value is null or whitespace, otherwise returns the value.
        /// </summary>
        /// <param name="value"></param>
        public static string GetNormalizedValue(string value) => string.IsNullOrWhiteSpace(value) ? null : value;

        public static string GetToString(IComponentProperty componentProperty)
        {
            if (componentProperty?.Value == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            builder.Append(componentProperty.Name);
            AppendProperties(componentProperty.Properties, builder);
            builder.Append($":{componentProperty.Value}");
            return builder.ToString();
        }

        private static readonly StringComparer _defaultComparer = StringComparer.Ordinal;

        public static IReadOnlyList<string> GetNormalizedStringCollection(IEnumerable<string> strings)
            => GetNormalizedStringCollection(strings, _defaultComparer);

        public static IReadOnlyList<string> GetNormalizedStringCollection(IEnumerable<string> strings, StringComparer comparerOverride)
        {
            if (strings == null)
            {
                return null;
            }

            var normalized = strings
                .Distinct(comparerOverride)
                .Where(c => !string.IsNullOrEmpty(c))
                .OrderBy(c => c, comparerOverride)
                .ToList();

            return normalized.Count == 0
                ? null
                : normalized.AsReadOnly();
        }

        /// <summary>
        /// {Foo,Bar,Baz} becomes ";Foo;Bar;Baz". If the collection is null or empty, nothing is appended.
        /// </summary>
        public static void AppendProperties(IEnumerable<string> additionalProperties, StringBuilder builder)
        {
            if (additionalProperties == null)
            {
                return;
            }

            foreach (var property in additionalProperties)
            {
                builder.Append($";{property}");
            }
        }
    }
}
