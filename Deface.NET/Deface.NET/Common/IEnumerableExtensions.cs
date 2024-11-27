using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.Common;

[ExcludeFromCodeCoverage]
internal static class IEnumerableExtensions
{
    public static void Dispose<T>(this IEnumerable<T> collection) where T : IDisposable
    {
        foreach (var item in collection)
        {
            item?.Dispose();
        }
    }
}
