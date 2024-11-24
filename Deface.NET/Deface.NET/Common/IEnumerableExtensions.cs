namespace Deface.NET.Common;

internal static class IEnumerableExtensions
{
    public static void Dispose<T>(this IEnumerable<T> collection) where T : IDisposable
    {
        foreach (var item in collection)
        {
            if (item is not null)
            {
                item.Dispose();
            }
        }
    }
}
