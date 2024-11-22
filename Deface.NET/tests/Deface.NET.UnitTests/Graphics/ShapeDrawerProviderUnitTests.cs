using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics;
using Deface.NET.Graphics.Effects;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.Graphics;

[Collection(nameof(SettingsCollection))]
public class ShapeDrawerProviderUnitTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void ShapeDrawer_ShouldBeGaussianBlurShapeDrawer()
    {
        var settingsProvider = GetSettingsProvider(AnonimizationMethod.GaussianBlur);

        ShapeDrawerProvider provider = new(settingsProvider);

        provider.ShapeDrawer.Should().BeAssignableTo<GaussianBlurShapeDrawer>();
    }

    [Fact]
    public void ShapeDrawer_ShouldBeMosaicShapeDrawer()
    {
        var settingsProvider = GetSettingsProvider(AnonimizationMethod.Mosaic);

        ShapeDrawerProvider provider = new(settingsProvider);

        provider.ShapeDrawer.Should().BeAssignableTo<MosaicShapeDrawer>();
    }

    [Fact]
    public void ShapeDrawer_ShouldBeColorShapeDrawer()
    {
        var settingsProvider = GetSettingsProvider(AnonimizationMethod.Color(0, 0, 0));

        ShapeDrawerProvider provider = new(settingsProvider);

        provider.ShapeDrawer.Should().BeAssignableTo<ColorShapeDrawer>();
    }

    private IScopedSettingsProvider GetSettingsProvider(AnonimizationMethod method)
    {
        var settings = _settingsFixture.WithAction(x =>
        {
            x.AnonimizationMethod = method;
        });

        var settingsProvider = Substitute.For<IScopedSettingsProvider>();
        settingsProvider.Settings.Returns(settings);

        return settingsProvider;
    }
}
