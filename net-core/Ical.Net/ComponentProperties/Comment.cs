using System;
using System.Collections.Generic;
using System.Text;

namespace Ical.Net.ComponentProperties
{
    /// <summary>
    /// This property specifies non-processing information intended to provide a comment to the calendar user.
    ///
    /// ANA, non-standard, alternate text representation, and language property parameters can be specified on this property.
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.4
    /// </summary>
    public class Comment :
        IComponentProperty
    {
        public string Name => "COMMENT";
        public string Value { get; }

        public Comment(string comment)
        {
            Value = string.IsNullOrWhiteSpace(comment)
                ? null
                : comment;
        }

        public override string ToString() => Value == null ? null : $"{Name}:{Value}";
    }
}
