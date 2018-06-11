using System;
using System.Collections.Generic;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property defines the overall status or confirmation for the calendar component, and can be specified once in "VEVENT", "VTODO",
    /// or "VJOURNAL" calendar components.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.11
    /// </summary>
    public class Status :
        INameValueProperty
    {
        public string Name => "STATUS";
        public string Value { get; }
        public IReadOnlyList<string> Properties { get; }

        /// <summary>
        /// Must be a valid VEVENT, VTODO, or VJOURNAL STATUS value
        /// </summary>
        public Status(string status, IEnumerable<string> additionalProperties = null)
        {
            var isValid = EventStatus.IsValid(status)
                || TodoStatus.IsValid(status)
                || JournalStatus.IsValid(status);

            if (!isValid)
            {
                throw new ArgumentException($"{status} is not an allowed VEVENT, VTODO, or VJOURNAL STATUS value");
            }

            Value = status;
            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalProperties);
        }
        
        public Status(string status)
            : this(status, null) { }

        public override string ToString() => SerializationUtilities.GetToString(this);
    }

    /// <summary>
    /// Represents valid status values for a VEVENT, which are TENTATIVE, CONFIRMED, or CANCELLED
    /// </summary>
    public static class EventStatus
    {
        public static string Tentative => "TENTATIVE";
        public static string Confirmed => "CONFIRMED";
        public static string Cancelled => "CANCELLED";

        private static readonly HashSet<string> _allowedValues = new HashSet<string>(StringComparer.Ordinal)
        {
            Tentative, Confirmed, Cancelled,
        };

        public static bool IsValid(string eventStatus) => _allowedValues.Contains(eventStatus);
    }

    /// <summary>
    /// Represents valid status values for a VTODO, which are NEEDS-ACTION, COMPLETED, IN-PROCESS, and CANCELLED
    /// </summary>
    public static class TodoStatus
    {
        public static string NeedsAction => "NEEDS-ACTION";
        public static string Completed => "COMPLETED";
        public static string InProcess => "IN-PROCESS";
        public static string Cancelled => "CANCELLED";

        private static readonly HashSet<string> _allowedValues = new HashSet<string>(StringComparer.Ordinal)
        {
            NeedsAction, Completed, InProcess, Cancelled,
        };

        public static bool IsValid(string todoStatus) => _allowedValues.Contains(todoStatus);
    }

    /// <summary>
    /// Represents valid status values for a VJOURNAL, which are DRAFT, FINAL, CANCELLED
    /// </summary>
    public static class JournalStatus
    {
        public static string Draft => "DRAFT";
        public static string Final => "FINAL";
        public static string Cancelled => "CANCELLED";

        private static readonly HashSet<string> _allowedValues = new HashSet<string>(StringComparer.Ordinal)
        {
            Draft, Final, Cancelled,
        };

        public static bool IsValid(string todoStatus) => _allowedValues.Contains(todoStatus);
    }
}
