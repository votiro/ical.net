using System;
using System.Collections.Generic;
using System.Globalization;
using Experiments.ComponentProperties;
using NodaTime;

namespace Experiments.PropertyValues
{
    /// <summary>
    /// This value type is used to identify values that contain a calendar date.
    ///
    /// If the property permits, multiple "date" values are specified as a COMMA-separated list of values.The format for the value type is based on
    /// the[ISO.8601.2004] complete representation, basic format for a calendar date. The textual format specifies a four-digit year, two-digit month,
    /// and two-digit day of the month.There are no separator characters between the year, month, and day component text.
    ///
    /// No additional content value encoding (i.e., BACKSLASH character encoding, see Section 3.3.11) is defined for this value type. 
    /// https://tools.ietf.org/html/rfc5545#section-3.3.4
    /// </summary>
    public struct Date :
        INameValueProperty
    {
        public string Name => "DATE";
        public string Value => DateValue.ToString("yyyyMMdd", CultureInfo.CurrentCulture);
        public LocalDate DateValue { get; }
        public IReadOnlyList<string> Properties => null;

        public Date(LocalDate localDate)
        {
            DateValue = localDate;
        }

        /// <summary>
        /// Initializes a Date struct using the Date property of the .NET DateTime struct. No conversion to any time zone is performed.
        /// </summary>
        public Date(DateTime localDate)
            : this(LocalDate.FromDateTime(localDate)) { }

        public override string ToString() => Value;
    }
}