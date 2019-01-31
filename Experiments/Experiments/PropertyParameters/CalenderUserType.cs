using System;
using System.Collections.Generic;
using Experiments.ComponentProperties;

namespace Experiments.PropertyParameters
{
    /// <summary>
    /// This parameter can be specified on properties with a CAL-ADDRESS value type.The parameter identifies the type of calendar user specified by the
    /// property.If not specified on a property that allows this parameter, the default is INDIVIDUAL. Applications MUST treat x-name and iana-token values
    /// they don't recognize the same way as they would the UNKNOWN value.
    /// https://tools.ietf.org/html/rfc5545#section-3.2.3
    /// </summary>
    public struct CalenderUserType :
        INameValueProperty
    {
        public string Name => "CUTYPE";
        public string Value { get; }
        public IReadOnlyList<string> Properties => null;

        public CalenderUserType(string userType)
        {
            if (!CalendarUserTypes.IsValid(userType))
            {
                throw new ArgumentException($"{userType} isn't a recognized calendar user type.");
            }

            Value = userType;
        }
    }

    
}