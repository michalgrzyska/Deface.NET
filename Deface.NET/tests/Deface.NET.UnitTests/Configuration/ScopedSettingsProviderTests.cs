using Deface.NET.Configuration;
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

        provider.Settings.FaceThreshold.ShouldBe(settings.FaceThreshold);
        provider.Settings.LicensePlateThreshold.ShouldBe(settings.LicensePlateThreshold);
    }

    [Fact]
    public void CustomScopedSettings_AreValuesOk()
    {
        float threshold = 0.99f;
        float licensePlateThreshold = 0.88f;

        var provider = GetScopedSettingsProvider();

        provider.LoadForCurrentScope(ProcessingType.Image, x =>
        {
            x.FaceThreshold = threshold;
            x.LicensePlateThreshold = licensePlateThreshold;
        });

        provider.Settings.FaceThreshold.ShouldBe(threshold);
        provider.Settings.LicensePlateThreshold.ShouldBe(licensePlateThreshold);
    }

    [Fact]
    public void CustomScopedSettings_InitiatedTwice_ShouldBeInitatedOnlyOnce()
    {
        float threshold = 0.3f;
        float licensePlateThreshold = 0.88f;

        var provider = GetScopedSettingsProvider();

        provider.LoadForCurrentScope(ProcessingType.Image, x =>
        {
            x.FaceThreshold = threshold;
            x.LicensePlateThreshold = licensePlateThreshold;
        });

        provider.LoadForCurrentScope(ProcessingType.Image, x =>
        {
            x.FaceThreshold = 0.4f;
            x.LicensePlateThreshold = 0.5f;
        });

        provider.Settings.FaceThreshold.ShouldBe(threshold);
        provider.Settings.LicensePlateThreshold.ShouldBe(licensePlateThreshold);
    }

    private ScopedSettingsProvider GetScopedSettingsProvider(Action<Settings>? action = null)
    {
        var settings = _settingsFixture.Settings;

        var settingsProvider = Substitute.For<ISettingsProvider>();
        settingsProvider.Settings.Returns(settings);

        return new(settingsProvider, Substitute.For<ISettingsValidator>());
    }
}
