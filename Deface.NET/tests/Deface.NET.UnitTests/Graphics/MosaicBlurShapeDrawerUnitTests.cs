using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests.Graphics;

[Collection(nameof(SettingsCollection))]
public class MosaicBlurShapeDrawerUnitTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;
}
