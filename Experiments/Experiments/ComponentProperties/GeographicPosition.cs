using System;
using System.Collections.Generic;
using System.Text;

namespace Experiments.ComponentProperties
{
    /// <summary>
    /// This property specifies information related to the global position for the activity specified by a calendar component.
    ///
    /// https://tools.ietf.org/html/rfc5545#section-3.8.1.6
    /// </summary>
    public class GeographicPosition
        : IComponentProperty
    {
        /// <summary>
        /// Must be between -90 and +90
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Must be between -180 and +180
        /// </summary>
        public double Longitude { get; }

        public string Name => "GEO";
        
        /// <summary>
        /// Returns the formatted latitude and longitude rounded to a precision of 6 decimal places
        /// </summary>
        public string Value => FormatValue();
        public IReadOnlyList<string> Properties { get; }

        /// <summary>
        /// The longitude and latitude values MAY be specified up to six decimal places, which will allow for accuracy to within one meter of
        /// geographical position. Additional precision may be truncated by other applications.
        /// </summary>
        /// <param name="latitude">Must be between -90 and +90</param>
        /// <param name="longitude">must be between -180 and +180</param>
        public GeographicPosition(double latitude, double longitude, IEnumerable<string> additionalProperties)
        {
            if (latitude > 90d || latitude < -90d)
            {
                throw new ArgumentException("Latitude must be between -90 and +90");
            }

            if (longitude > 180d || longitude < -180d)
            {
                throw new ArgumentException("Latitude must be between -180 and +180");
            }

            Latitude = latitude;
            Longitude = longitude;
            Properties = ComponentPropertiesUtilities.GetNormalizedStringCollection(additionalProperties);
        }

        private string _value;
        private string FormatValue()
        {
            if (_value == null)
            {
                var builder = new StringBuilder(27);
                builder.Append($"{Math.Round(Latitude, 6, MidpointRounding.AwayFromZero)}");
                builder.Append($";{Math.Round(Longitude, 6, MidpointRounding.AwayFromZero)}");
                _value = builder.ToString();
            }

            return _value;
        }

        public override string ToString() => ComponentPropertiesUtilities.GetToString(this);
    }
}
