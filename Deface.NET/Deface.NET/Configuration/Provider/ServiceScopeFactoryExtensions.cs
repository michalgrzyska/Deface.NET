using Deface.NET.CommercialFeatures.Interfaces;
using Deface.NET.Configuration.Provider.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET.Configuration.Provider;

internal static class ServiceScopeFactoryExtensions
{
    public static IServiceScope CreateUserScope(
        this IServiceScopeFactory serviceScopeFactory, 
        ProcessingType processingType, 
        Action<Settings>? scopedSettings)
    {
        var scope = serviceScopeFactory.CreateScope();

        var scopedSettingsProvider = scope.ServiceProvider.GetRequiredService<IScopedSettingsProvider>();
        scopedSettingsProvider.LoadForCurrentScope(processingType, scopedSettings);

        var commercialFeaturesReporter = scope.ServiceProvider.GetRequiredService<ICommercialFeaturesReporter>();
        commercialFeaturesReporter.ReportCommercialFeatures();

        return scope;
    }
}
