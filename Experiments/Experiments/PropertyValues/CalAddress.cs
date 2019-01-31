using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Experiments.ComponentProperties;

namespace Experiments.PropertyValues
{
    /// <summary>
    /// The value is a URI as defined by [RFC3986] or any other IANA-registered form for a URI. When used to address an Internet email transport address for a
    /// calendar user, the value MUST be a mailto URI, as defined by[RFC2368].  No additional content value encoding (i.e., BACKSLASH character encoding,
    /// see Section 3.3.11) is defined for this value type.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.3.3
    /// </summary>
    public class CalAddress :
        INameValueProperty
    {
        public string Name => "CAL-ADDRESS";
        public Uri UriValue { get; }
        public string Value => ToString();
        public string CommonName { get; }
        public string CalendarUserType { get; }
        public IReadOnlyList<string> Properties => null;

        public CalAddress(Uri uri, string commonName, string calendarUserType)
        {
            UriValue = uri;
            CommonName = commonName;
            CalendarUserType = calendarUserType;
        }

        public CalAddress(Uri uri)
            : this(uri, commonName: null, calendarUserType: null) { }

        public CalAddress(Uri uri, string commonName)
            : this(uri, commonName: commonName, calendarUserType: null) { }

        public CalAddress(Uri uri, string calendarUserType)
            : this(uri, commonName: null, calendarUserType: calendarUserType) { }


        //Uri
        //Uri + CN
        //URI + cut
        //URI + CN + cut

        public CalAddress(Uri uri, )

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(CommonName))
            {
                builder.Append($"CN=\"{CommonName}\":");
            }

            builder.Append(UriValue.AbsoluteUri);
            return builder.ToString();
        }
    }

    /// <summary>
    /// Represents valid status values for a VEVENT, which are TENTATIVE, CONFIRMED, or CANCELLED
    /// </summary>
    public static class CalendarUserTypes
    {
        public static string Default => Individual;
        public static string Individual => "INDIVIDUAL";
        public static string Group => "GROUP";
        public static string Resource => "RESOURCE";
        public static string Room => "ROOM";
        public static string Unknown => "UNKNOWN";

        private static readonly HashSet<string> _allowedValues = new HashSet<string>(StringComparer.Ordinal)
        {
            Individual, Group, Resource, Room, Unknown,
        };

        public static bool IsValid(string userType)
            => userType.StartsWith("X-", StringComparison.Ordinal) || _allowedValues.Contains(userType);
    }
}
