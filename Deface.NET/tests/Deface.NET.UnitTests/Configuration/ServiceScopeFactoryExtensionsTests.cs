using Deface.NET.CommercialFeatures.Interfaces;
using Deface.NET.Configuration;
using Deface.NET.Configuration.Provider;
using Deface.NET.Configuration.Provider.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Deface.NET.UnitTests.Configuration;

public class ServiceScopeFactoryExtensionsTests
{
    [Fact]
    public void CreateUserScope_WithCustomSettings_ShouldCallInitOnScopedSettingsProvider()
    {
        // Arrange

        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        var serviceScope = Substitute.For<IServiceScope>();
        var scopedSettingsProvider = Substitute.For<IScopedSettingsProvider>();
        var commercialFeaturesReporter = Substitute.For<ICommercialFeaturesReporter>();

        serviceScopeFactory.CreateScope().Returns(serviceScope);
        serviceScope.ServiceProvider.GetService(typeof(IScopedSettingsProvider)).Returns(scopedSettingsProvider);
        serviceScope.ServiceProvider.GetService(typeof(ICommercialFeaturesReporter)).Returns(commercialFeaturesReporter);


        Action<Settings> customSettings = _ => { };

        // Act

        var result = serviceScopeFactory.CreateUserScope(ProcessingType.Video, customSettings);

        // Assert

        result.ShouldBe(serviceScope);
        scopedSettingsProvider.Received(1).LoadForCurrentScope(Arg.Any<ProcessingType>(), customSettings);
    }

    [Fact]
    public void CreateUserScope_WithoutCustomSettings_ShouldNotCallInitOnScopedSettingsProvider()
    {
        // Arrange

        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        var serviceScope = Substitute.For<IServiceScope>();
        var scopedSettingsProvider = Substitute.For<IScopedSettingsProvider>();
        var commercialFeaturesReporter = Substitute.For<ICommercialFeaturesReporter>();

        serviceScopeFactory.CreateScope().Returns(serviceScope);

        serviceScope.ServiceProvider.GetService(typeof(IScopedSettingsProvider)).Returns(scopedSettingsProvider);
        serviceScope.ServiceProvider.GetService(typeof(ICommercialFeaturesReporter)).Returns(commercialFeaturesReporter);

        // Act

        var result = serviceScopeFactory.CreateUserScope(ProcessingType.Video, null);

        // Assert

        result.ShouldBe(serviceScope);
        scopedSettingsProvider.Received(1).LoadForCurrentScope(Arg.Any<ProcessingType>(), Arg.Any<Action<Settings>>());
    }
}