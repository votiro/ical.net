using System;
using System.Collections.Generic;

namespace Ical.Net.ComponentProperties
{
    /// <summary>
    /// This property is used by an assignee or delegatee of a to-do to convey the percent completion of a to-do to the "Organizer".
    /// 
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.8
    /// </summary>
    public class PercentComplete :
        IComponentProperty
    {
        public string Name { get; }
        public string Value => CompletionPercent.ToString();
        public int CompletionPercent { get; }
        public IReadOnlyList<string> Properties { get; }

        /// <param name="completionPercent">Must be between 0 and 100</param>
        public PercentComplete(int completionPercent)
        {
            if (completionPercent < 0 || completionPercent > 100)
            {
                throw new ArgumentException("Value must be between 0 and 100");
            }

            CompletionPercent = completionPercent;
        }

        /// <summary>
        /// PERCENT-COMPLETE:39 would indicate that the VTODO is 39% complete
        /// </summary>
        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}
