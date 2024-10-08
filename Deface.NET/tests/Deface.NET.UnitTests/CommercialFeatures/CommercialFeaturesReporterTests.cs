using Deface.NET.CommercialFeatures;
using Deface.NET.Logging;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.CommercialFeatures;

[Collection(nameof(SettingsCollection))]
public class CommercialFeaturesReporterTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void ReportCommercialFeatures_HideCommercialFeaturesInfoFalse_NoLoggingExecuted()
    {
        // Arrange

        var logger = Substitute.For<IDLogger<IDefaceService>>();

        var scopedSettingsProvider = _settingsFixture.GetScopedSettingsProvider(x =>
        {
            x.HideCommercialFeaturesInfo = true;
            x.EncodingCodec = EncodingCodec.H264;
        });

        CommercialFeaturesReporter reporter = new(logger, scopedSettingsProvider);

        // Act

        reporter.ReportCommercialFeatures();

        // Assert

        logger.DidNotReceive().LogBasic(Arg.Any<string>());
    }

    [Fact]
    public void ReportCommercialFeatures_OnlyOpenSourceFeaturesSelected_NoLoggingExecuted()
    {
        // Arrange

        var logger = Substitute.For<IDLogger<IDefaceService>>();

        var scopedSettingsProvider = _settingsFixture.GetScopedSettingsProvider(x =>
        {
            x.EncodingCodec = EncodingCodec.VP9;
        });

        CommercialFeaturesReporter reporter = new(logger, scopedSettingsProvider);

        // Act

        reporter.ReportCommercialFeatures();

        // Assert

        logger.DidNotReceive().LogBasic(Arg.Any<string>());
    }

    [Fact]
    public void ReportCommercialFeatures_CommercialFeaturesSelected_LogsCommercialFeatures()
    {
        // Arrange

        var logger = Substitute.For<IDLogger<IDefaceService>>();

        var scopedSettingsProvider = _settingsFixture.GetScopedSettingsProvider(x =>
        {
            x.EncodingCodec = EncodingCodec.H264;
        });

        CommercialFeaturesReporter reporter = new(logger, scopedSettingsProvider);

        // Act

        reporter.ReportCommercialFeatures();

        // Assert

        logger.Received().LogBasic(Arg.Any<string>());
    }
}
