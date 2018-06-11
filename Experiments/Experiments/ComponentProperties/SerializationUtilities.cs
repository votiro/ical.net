using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Experiments.Utilities;
using Experiments.ValueTypes;
using NodaTime;

namespace Experiments.ComponentProperties
{
    public static class SerializationUtilities
    {
        /// <summary>
        /// Returns null if the value is null or whitespace, otherwise returns the value.
        /// </summary>
        /// <param name="value"></param>
        public static string GetNormalizedValue(string value) => string.IsNullOrWhiteSpace(value) ? null : value;

        public static string GetToString(INameValueProperty nameValueProperty)
        {
            if (nameValueProperty?.Value == null)
            {
                return "";
            }

            var builder = new StringBuilder();
            builder.Append(nameValueProperty.Name);
            AppendProperties(nameValueProperty.Properties, builder);
            builder.Append($":{nameValueProperty.Value}");
            builder.Append(SerializationConstants.LineBreak);
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

        public static void AppendDateTime(StringBuilder builder, IDateTime dt)
        {
            var dateOrDateTime = dt.HasTime ? "DATE-TIME" : "DATE";
            builder.Append($"{dt.Name};VALUE={dateOrDateTime}");

            // DATEs are, by definition, don't have time zones.
            // If the time zone is UTC, we must serialize it with a Z instead of a TZID parameter
            if (dt.HasTime && dt.TimeZone != null && dt.TimeZone != DateTimeZone.Utc)
            {
                builder.Append($"{dt.TzIdKey}={dt.TimeZoneName}");
            }

            AppendProperties(dt.Properties, builder);
            var formattedDateOrDateTime = dt.HasTime
                ? $"{LocalDateTimeToString(dt.Date, dt.Time)}"
                : $"{LocalDateToString(dt.Date)}";
            builder.Append($";{formattedDateOrDateTime}");
        }

        public static string LocalDateToString(LocalDate localDate)
            => $"{localDate.Year:0000}{localDate.Month:00}{localDate.Day:00}";

        public static string LocalTimeToString(LocalTime localTime)
            => $"{localTime.Hour:00}{localTime.Minute:00}{localTime.Second:00}";
        
        public static string LocalDateTimeToString(LocalDate localDate, LocalTime localTime)
            => $"{LocalDateToString(localDate)}T{LocalTimeToString(localTime)}";
    }
}
