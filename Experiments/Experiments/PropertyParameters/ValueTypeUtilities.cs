namespace Experiments.PropertyParameters
{
    public static class ValueTypeUtilities
    {
        public static string GetToString(IValueType valueType) => valueType.IsEmpty ? "" : $"{valueType.Name}:{valueType.Value}";
    }
}