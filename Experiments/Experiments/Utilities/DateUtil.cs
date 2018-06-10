using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NodaTime;
using NodaTime.TimeZones;

namespace Experiments.Utilities
{
    public static class DateUtil
    {
        private static readonly Lazy<Dictionary<string, string>> _windowsMapping
            = new Lazy<Dictionary<string, string>>(InitializeWindowsMappings, LazyThreadSafetyMode.PublicationOnly);

        private static Dictionary<string, string> InitializeWindowsMappings()
            => TzdbDateTimeZoneSource.Default.WindowsMapping.PrimaryMapping
                .ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);

        public static readonly DateTimeZone SystemTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();

        /// <summary>
        /// Use this method to turn a raw string into a NodaTime DateTimeZone. It searches all time zone providers (IANA, BCL, serialization, etc) to see if
        /// the string matches. If it doesn't, it walks each provider, and checks to see if the time zone the provider knows about is contained within the
        /// target time zone string. Some older icalendar programs would generate nonstandard time zone strings, and this secondary check works around
        /// that.
        /// </summary>
        /// <param name="tzId">A BCL, IANA, or serialization time zone identifier</param>
        /// <param name="useLocalIfNotFound">If true, this method will return the system local time zone if tzId doesn't match a known time zone identifier.
        /// Otherwise, it will throw an exception.</param>
        public static DateTimeZone GetZone(string tzId)
        {
            if (string.IsNullOrWhiteSpace(tzId))
            {
                return SystemTimeZone;
            }

            if (tzId.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                tzId = tzId.Substring(1, tzId.Length - 1);
            }

            var zone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(tzId);
            if (zone != null)
            {
                return zone;
            }

            if (_windowsMapping.Value.TryGetValue(tzId, out var ianaZone))
            {
                return DateTimeZoneProviders.Tzdb.GetZoneOrNull(ianaZone);
            }

            zone = DateTimeZoneProviders.Serialization.GetZoneOrNull(tzId);
            if (zone != null)
            {
                return zone;
            }

            //US/Eastern is commonly represented as US-Eastern
            var newTzId = tzId.Replace("-", "/");
            zone = DateTimeZoneProviders.Serialization.GetZoneOrNull(newTzId);
            if (zone != null)
            {
                return zone;
            }

            foreach (var providerId in DateTimeZoneProviders.Tzdb.Ids.Where(tzId.Contains))
            {
                return DateTimeZoneProviders.Tzdb.GetZoneOrNull(providerId);
            }

            if (_windowsMapping.Value.Keys
                    .Where(tzId.Contains)
                    .Any(providerId => _windowsMapping.Value.TryGetValue(providerId, out ianaZone))
               )
            {
                return DateTimeZoneProviders.Tzdb.GetZoneOrNull(ianaZone);
            }

            foreach (var providerId in DateTimeZoneProviders.Serialization.Ids.Where(tzId.Contains))
            {
                return DateTimeZoneProviders.Serialization.GetZoneOrNull(providerId);
            }

            throw new ArgumentException($"Unrecognized time zone id {tzId}");
        }

        public static ZonedDateTime AddYears(ZonedDateTime zonedDateTime, int years)
        {
            var futureDate = zonedDateTime.Date.PlusYears(years);
            var futureLocalDateTime = new LocalDateTime(futureDate.Year, futureDate.Month, futureDate.Day, zonedDateTime.Hour, zonedDateTime.Minute,
                zonedDateTime.Second);
            var zonedFutureDate = new ZonedDateTime(futureLocalDateTime, zonedDateTime.Zone, zonedDateTime.Offset);
            return zonedFutureDate;
        }

        public static ZonedDateTime AddMonths(ZonedDateTime zonedDateTime, int months)
        {
            var futureDate = zonedDateTime.Date.PlusMonths(months);
            var futureLocalDateTime = new LocalDateTime(futureDate.Year, futureDate.Month, futureDate.Day, zonedDateTime.Hour, zonedDateTime.Minute,
                zonedDateTime.Second);
            var zonedFutureDate = new ZonedDateTime(futureLocalDateTime, zonedDateTime.Zone, zonedDateTime.Offset);
            return zonedFutureDate;
        }

        public static ZonedDateTime ToZonedDateTimeLeniently(DateTime dateTime, string tzId)
        {
            var zone = GetZone(tzId);
            var localDt = LocalDateTime.FromDateTime(dateTime); //19:00 UTC
            var lenientZonedDateTime = localDt.InZoneLeniently(zone).WithZone(zone); //15:00 Eastern
            return lenientZonedDateTime;
        }

        public static ZonedDateTime ToZonedDateTimeLeniently(DateTime dateTime, DateTimeZone zone)
        {
            var lenientDt = LocalDateTime.FromDateTime(dateTime);
            var lenientZoned = lenientDt.InZoneLeniently(zone);
            return lenientZoned;
        }

        public static ZonedDateTime ToZonedDateTimeLeniently(DateTimeOffset dateTimeOffset, DateTimeZone zone)
        {
            var instant = Instant.FromDateTimeOffset(dateTimeOffset);
            var lenientZoned = new ZonedDateTime(instant, zone);
            return lenientZoned;
        }

        public static ZonedDateTime FromTimeZoneToTimeZone(DateTime dateTime, string fromZoneId, string toZoneId)
            => FromTimeZoneToTimeZone(dateTime, GetZone(fromZoneId), GetZone(toZoneId));

        public static ZonedDateTime FromTimeZoneToTimeZone(DateTime dateTime, DateTimeZone fromZone, DateTimeZone toZone)
        {
            var oldZone = LocalDateTime.FromDateTime(dateTime).InZoneLeniently(fromZone);
            var newZone = oldZone.WithZone(toZone);
            return newZone;
        }

        public static bool IsSerializationTimeZone(DateTimeZone zone) => DateTimeZoneProviders.Serialization.GetZoneOrNull(zone.Id) != null;

        /// <summary>
        /// Truncate to the specified TimeSpan's magnitude. For example, to truncate to the nearest second, use TimeSpan.FromSeconds(1)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
            => timeSpan == TimeSpan.Zero
                ? dateTime
                : dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));

        public static int WeekOfMonth(DateTime d)
        {
            var isExact = d.Day % 7 == 0;
            var offset = isExact
                ? 0
                : 1;
            return (int) Math.Floor(d.Day / 7.0) + offset;
        }
    }
}