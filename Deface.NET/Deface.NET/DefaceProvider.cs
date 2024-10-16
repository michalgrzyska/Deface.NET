using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET;

/// <summary>
/// Provides <see cref="IDefaceService"/> for non-DI apps.
/// </summary>
public static class DefaceProvider
{
    /// <summary>
    /// Creates a new instance of <see cref="IDefaceService"/>. This is not recommended way
    /// of using Deface. If possible, use DI method instead.
    /// </summary>
    public static IDefaceService GetDefaceService(Action<Settings>? settings = null)
    {
        ServiceCollection services = new();
        services.AddDeface(settings);

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IDefaceService>();
    }
}