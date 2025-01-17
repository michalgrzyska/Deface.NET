using Deface.NET.Configuration.Provider;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Configuration.Validation;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.Configuration;

[Collection(nameof(SettingsCollection))]
public class ScopedSettingsProviderTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void DefaultSettings_IsASampleDefaultValueOk()
    {
        var settings = _settingsFixture.Settings;
        var provider = GetScopedSettingsProvider();

        provider.Settings.Threshold.ShouldBe(settings.Threshold);
    }

    [Fact]
    public void CustomScopedSettings_AreValuesOk()
    {
        float threshold = 0.99f;

        var provider = GetScopedSettingsProvider();

        provider.Init(x =>
        {
            x.Threshold = threshold;
        });

        provider.Settings.Threshold.ShouldBe(threshold);
    }

    [Fact]
    public void CustomScopedSettings_InitiatedTwice_ShouldBeInitatedOnlyOnce()
    {
        float threshold = 0.3f;

        var provider = GetScopedSettingsProvider();

        provider.Init(x =>
        {
            x.Threshold = threshold;
        });

        provider.Init(x =>
        {
            x.Threshold = 0.4f;
        });

        provider.Settings.Threshold.ShouldBe(threshold);
    }

    private ScopedSettingsProvider GetScopedSettingsProvider(Action<Settings>? action = null)
    {
        var settings = _settingsFixture.Settings;

        var settingsProvider = Substitute.For<ISettingsProvider>();
        settingsProvider.Settings.Returns(settings);

        return new(settingsProvider, Substitute.For<ISettingsValidator>());
    }
}
