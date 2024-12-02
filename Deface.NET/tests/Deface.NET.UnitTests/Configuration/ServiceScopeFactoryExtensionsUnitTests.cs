using Deface.NET.Configuration.Provider;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Deface.NET.UnitTests.Configuration;

public class ServiceScopeFactoryExtensionsUnitTests
{
    [Fact]
    public void CreateUserScope_WithCustomSettings_ShouldCallInitOnScopedSettingsProvider()
    {
        // Arrange

        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        var serviceScope = Substitute.For<IServiceScope>();
        var scopedSettingsProvider = Substitute.For<IScopedSettingsProvider>();

        serviceScopeFactory.CreateScope().Returns(serviceScope);
        serviceScope.ServiceProvider.GetService(typeof(IScopedSettingsProvider)).Returns(scopedSettingsProvider);

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

        serviceScopeFactory.CreateScope().Returns(serviceScope);
        serviceScope.ServiceProvider.GetService(typeof(IScopedSettingsProvider)).Returns(scopedSettingsProvider);

        // Act

        var result = serviceScopeFactory.CreateUserScope(null);

        // Assert

        result.Should().Be(serviceScope);
        scopedSettingsProvider.DidNotReceive().Init(Arg.Any<Action<Settings>>());
    }
}