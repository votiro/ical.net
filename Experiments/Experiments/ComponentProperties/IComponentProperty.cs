using System.Collections.Generic;

namespace Experiments.ComponentProperties
{
    public interface IComponentProperty
    {
        /// <summary>
        /// Every property has a name like ATTACH or COMMENT which is the identifier for the type being serialized or deserialized.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Returns the RFC-compliant serialized form of the property
        /// </summary>
        /// <returns></returns>
        string ToString();

        /// <summary>
        /// Optional properties, IANA or otherwise
        /// </summary>
        IReadOnlyList<string> Properties { get; }
    }
}