using Deface.NET.CommercialFeatures.Features;
using Deface.NET.CommercialFeatures.Interfaces;
using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests.CommercialFeatures;

[Collection(nameof(SettingsCollection))]
public class H264CommercialFeatureTests(SettingsFixture settingsFixture) : ICommercialFeatureBaseTests
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    internal override Settings EnabledSettings => _settingsFixture.WithAction(x => x.EncodingCodec = EncodingCodec.H264);
    internal override Settings DisabledSettings => _settingsFixture.WithAction(x => x.EncodingCodec = EncodingCodec.VP9);
    internal override ICommercialFeature GetCommercialFeature() => new H264CommercialFeature();
}
