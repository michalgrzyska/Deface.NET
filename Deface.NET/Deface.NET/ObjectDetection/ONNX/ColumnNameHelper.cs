using Microsoft.ML.Data;
using System.Reflection;

namespace Deface.NET.ObjectDetection.ONNX;

internal static class ColumnNameHelper
{
    private const string NameProp = "Name";

    public static string[] GetColumnNamesFrom<T>() where T : class
    {
        var type = typeof(T);
        var properties = type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(ColumnNameAttribute)));
        var columnNames = properties.Select(GetColumnNameFromProperty);

        return [.. columnNames];
    }

    private static string GetColumnNameFromProperty(PropertyInfo x)
    {
        var attribute = x.GetCustomAttribute<ColumnNameAttribute>()!;
        var nameProperty = attribute.GetType().GetProperty(NameProp, BindingFlags.NonPublic | BindingFlags.Instance)!;
        var value = nameProperty.GetValue(attribute)!.ToString();

        return value!;
    }
}
