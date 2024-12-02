using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests;

[Collection(nameof(SettingsCollection))]
public class DefaceProviderTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void GetDefaceService_ShouldReturnIDefaceServiceInstance()
    {
        // Act

        var defaceService = DefaceProvider.GetDefaceService(_settingsFixture.Action);

        // Assert

        defaceService.Should().NotBeNull();
        defaceService.Should().BeAssignableTo<IDefaceService>();
    }

    [Fact]
    public void GetDefaceService_ShouldConfigureSettings()
    {
        // Arrange

        var action = _settingsFixture.Action;

        var settingsActionCalled = false;

        action += settings =>
        {
            settingsActionCalled = true;
        };

        // Act

        var defaceService = DefaceProvider.GetDefaceService(action);

        // Assert

        settingsActionCalled.Should().BeTrue();
        defaceService.Should().NotBeNull();
        defaceService.Should().BeAssignableTo<IDefaceService>();
    }
}