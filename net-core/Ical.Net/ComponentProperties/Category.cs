using System;
using System.Collections.Generic;
using System.Text;

namespace Ical.Net.ComponentProperties
{
    public class Category
    {
        public string Name => "CATEGORIES";
        public IReadOnlyList<string> Categories { get; }

        /// <summary>
        /// Category constructor. Categories that are null or whitespace will be filtered out.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="comparerOverride">Default StringComparer is OrdinalIgnoreCase</param>
        public Category(IEnumerable<string> categories, StringComparer comparerOverride)
        {
            Categories = ComponentPropertiesUtilities.GetNormalizedStringCollection(categories, comparerOverride);
        }

        public Category(IEnumerable<string> categories)
            : this(categories, StringComparer.OrdinalIgnoreCase) { }

        public override string ToString()
        {
            if (Categories == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            builder.Append($"{Name}:");
            builder.Append(string.Join(",", Categories));
            builder.Append("\r\n");
            return builder.ToString();
        }

        public static Category FromString(string serializedCategory)
        {
            return null;
            //if (string.IsNullOrWhiteSpace(serializedCategory))
            //{
            //    return null;
            //}

            // This can probably be implemented in terms of Span<T>...
        }
    }
}