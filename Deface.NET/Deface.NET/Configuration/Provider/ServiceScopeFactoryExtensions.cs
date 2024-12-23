using Deface.NET.CommercialFeatures.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET.Configuration.Provider;

internal static class ServiceScopeFactoryExtensions
{
    public static IServiceScope CreateUserScope(this IServiceScopeFactory serviceScopeFactory, Action<Settings>? customSettings)
    {
        var scope = serviceScopeFactory.CreateScope();

        var scopedSettingsProvider = scope.ServiceProvider.GetRequiredService<IScopedSettingsProvider>();
        scopedSettingsProvider.Init(customSettings);

        var commercialFeaturesReporter = scope.ServiceProvider.GetRequiredService<ICommercialFeaturesReporter>();
        commercialFeaturesReporter.ReportCommercialFeatures();

        return scope;
    }
}
