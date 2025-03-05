namespace Deface.NET.IntegrationTests.Helpers;

internal static class AssertingExtensions
{
    public static void WithInnerException<T>(this Exception ex, string message = null) where T : Exception
    {
        ex.InnerException.ShouldBeOfType<T>(message);
    }
}
