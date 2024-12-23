using Deface.NET.CommercialFeatures.Interfaces;
using Deface.NET.Configuration.Provider;
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

        var result = serviceScopeFactory.CreateUserScope(customSettings);

        // Assert

        result.Should().Be(serviceScope);
        scopedSettingsProvider.Received(1).Init(customSettings);
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

        var result = serviceScopeFactory.CreateUserScope(null);

        // Assert

        result.Should().Be(serviceScope);
        scopedSettingsProvider.Received(1).Init(Arg.Any<Action<Settings>>());
    }
}