using System;
using System.Collections.Generic;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property defines the relative priority for a calendar component.
    ///
    /// This priority is specified as an integer in the range 0 to 9.  A value of 0 specifies an undefined priority.A value of 1 is the
    /// highest priority.A value of 2 is the second highest priority.  Subsequent numbers specify a decreasing ordinal priority.A value
    /// of 9 is the lowest priority.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.9
    /// </summary>
    public class Priority :
        INameValueProperty,
        IComparable<Priority>
    {
        public string Name => "PRIORITY";
        public string Value => PriorityValue.ToString();
        public int PriorityValue { get; } 
        public IReadOnlyList<string> Properties { get; }

        /// <summary>
        /// Highest priority = 1. Lowest priority = 9. A value of 0 means unspecified.
        /// </summary>
        /// <param name="priority">A value between 0 and 9</param>
        public Priority(int priority, IEnumerable<string> additionalParameters = null)
        {
            if (priority < 0 || priority > 9)
            {
                throw new ArgumentException("Priority must be between 0 and 9.");
            }

            PriorityValue = priority;
            Properties = SerializationUtilities.GetNormalizedStringCollection(additionalParameters);
        }

        public Priority(int priority)
            : this(priority, null) { }

        /// <summary>
        /// Reverse ordering where 1 is weighted more heavily than 9, and 0 has the lowest possible priority.
        /// </summary>
        public int CompareTo(Priority other)
        {
            if (PriorityValue == 0)
            {
                return -1;
            }

            if (PriorityValue > other.PriorityValue)
            {
                return -1;
            }

            if (PriorityValue < other.PriorityValue)
            {
                return 1;
            }

            return 0;
        }

        public override string ToString() => SerializationUtilities.GetToString(this);
    }
}
