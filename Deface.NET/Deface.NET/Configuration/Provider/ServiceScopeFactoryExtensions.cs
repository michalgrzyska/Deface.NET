using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET.Configuration.Provider;

internal static class ServiceScopeFactoryExtensions
{
    public static IServiceScope CreateUserScope(this IServiceScopeFactory serviceScopeFactory, Action<Settings>? customSettings)
    {
        var scope = serviceScopeFactory.CreateScope();

        if (customSettings is not null)
        {
            var scopedSettingsProvider = scope.ServiceProvider.GetRequiredService<IScopedSettingsProvider>();
            scopedSettingsProvider.Init(customSettings);
        }

        return scope;
    }
}
