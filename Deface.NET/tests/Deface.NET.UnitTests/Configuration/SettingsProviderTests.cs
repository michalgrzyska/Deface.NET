
using Deface.NET.Configuration.Provider;

namespace Deface.NET.UnitTests.Configuration;

public class SettingsProviderTests
{
    [Fact]
    public void SettingsProperty_DataNotInitialized_ThrowsInvalidOperationException()
    {
        var settingsProvider = new SettingsProvider();

        Should.Throw<InvalidOperationException>(() => settingsProvider.Settings);
    }

    [Fact]
    public void SettingsProperty_DataAlreadyInitialized_ThrowsInvalidOperationException()
    {
        var settingsProvider = new SettingsProvider();

        settingsProvider.Initialize(settings => settings.AnonimizationMethod = AnonimizationMethod.GaussianBlur);

        var action = 
            () => settingsProvider.Initialize(settings => settings.AnonimizationMethod = AnonimizationMethod.Mosaic);

        action.ShouldThrow<InvalidOperationException>();
    }
}
