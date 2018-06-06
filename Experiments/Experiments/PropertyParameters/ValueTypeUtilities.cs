namespace Experiments.PropertyParameters
{
    public static class ValueTypeUtilities
    {
        public static string GetToString(IValueType valueType) => valueType.IsEmpty ? null : $"{valueType.Name}:{valueType.Value}";
    }
}