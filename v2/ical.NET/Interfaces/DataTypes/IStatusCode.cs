using System;

namespace Ical.Net.Interfaces.DataTypes
{
    public interface IStatusCode : IEncodableDataType, ICloneable
    {
        int[] Parts { get; }
        int Primary { get; }
        int Secondary { get; }
        int Tertiary { get; }
    }
}