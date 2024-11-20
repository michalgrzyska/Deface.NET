using Deface.NET.Configuration.Provider;
using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests.Configuration;

[Collection(nameof(SettingsCollection))]
public class ScopedSettingsProviderUnitTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void DefaultSettings_IsASampleDefaultValueOk()
    {
        var settings = _settingsFixture.Settings;
        ScopedSettingsProvider provider = new(settings);

        provider.Settings.Threshold.Should().Be(settings.Threshold);
    }

    [Fact]
    public void CustomScopedSettings_AreValuesOk()
    {
        float threshold = 0.99f;

        var settings = _settingsFixture.Settings;
        ScopedSettingsProvider provider = new(settings);

        provider.Init(x =>
        {
            x.Threshold = threshold;
        });

        provider.Settings.Threshold.Should().Be(threshold);
    }

    [Fact]
    public void CustomScopedSettings_InitiatedTwice_ShouldBeInitatedOnlyOnce()
    {
        float threshold = 0.3f;

        var settings = _settingsFixture.Settings;
        ScopedSettingsProvider provider = new(settings);

        provider.Init(x =>
        {
            x.Threshold = threshold;
        });

        provider.Init(x =>
        {
            x.Threshold = 0.4f;
        });

        provider.Settings.Threshold.Should().Be(threshold);
    }
}
