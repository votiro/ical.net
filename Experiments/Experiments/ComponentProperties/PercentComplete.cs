using System;
using System.Collections.Generic;

namespace Experiments.ComponentProperties
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
        public string Value => PercentValue.ToString();
        public int PercentValue { get; }
        public IReadOnlyList<string> Properties { get; }

        /// <param name="percent">Must be between 0 and 100</param>
        public PercentComplete(int percent, IEnumerable<string> additionalProperties = null)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ArgumentException("Value must be between 0 and 100");
            }

            PercentValue = percent;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        public PercentComplete(int percent)
            : this(percent, null) { }

        /// <summary>
        /// PERCENT-COMPLETE:39 would indicate that the VTODO is 39% complete
        /// </summary>
        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}
