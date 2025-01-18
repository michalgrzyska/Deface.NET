using Deface.NET.Configuration.Validation;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.Validation;

[Collection(nameof(SettingsCollection))]
public class SettingsValidatorUnitTests
{
    private readonly SettingsFixture _settingsFixture;

    private Settings Settings => _settingsFixture.Settings.Clone();

    public SettingsValidatorUnitTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(100.1)]
    [InlineData(101)]
    public void Threshold_IncorrectValues_ThrowsArgumentOutOfRangeException(float threshold)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.Threshold = threshold;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.25)]
    [InlineData(0.550)]
    [InlineData(0.7500001)]
    [InlineData(1)]
    public void Threshold_CorrectValues_NoExceptionThrown(float threshold)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.Threshold = threshold;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void RunDetectionEachNFrames_IncorrectData_ThrowsArgumentOutOfRangeException(int runDetectionEachNFrames)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.RunDetectionEachNFrames = runDetectionEachNFrames;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(27)]
    [InlineData(1000)]
    public void RunDetectionEachNFrames_CorrectData_NoExceptionThrown(int runDetectionEachNFrames)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.RunDetectionEachNFrames = runDetectionEachNFrames;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(0.99999)]
    [InlineData(0)]
    [InlineData(0.9)]
    [InlineData(0.1)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void MaskScale_IncorrectData_ThrowsArgumentOutOfRangeException(float maskScale)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.MaskScale = maskScale;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1.0f)]
    [InlineData(1.1f)]
    [InlineData(2f)]
    [InlineData(500f)]
    public void MaskScale_CorrectData_NoExceptionThrown(float maskScale)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.MaskScale = maskScale;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FFMpegPath_NullOrWhitespace_ArgumentNullExceptionThrown(string? value)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.FFMpegPath = value!;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldThrow<ArgumentNullException>();
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FFProbePath_NullOrWhitespace_ArgumentNullExceptionThrown(string? value)
    {
        var settings = Settings;

        settings.ApplyAction(settings =>
        {
            settings.FFProbePath = value!;
        });

        SettingsValidator validator = new(GetExternalProcessFactory(settings));

        var action = () => validator.Validate(settings);

        action.ShouldThrow<ArgumentNullException>();
    }

    private IExternalProcessFactory GetExternalProcessFactory(Settings settings)
    {
        var externalProcessFactory = Substitute.For<IExternalProcessFactory>();

        var ffmpegProcess = Substitute.For<IExternalProcess>();
        ffmpegProcess.ExecuteWithOutput().Returns("ffmpeg version 4.4.1");

        var ffprobeProcess = Substitute.For<IExternalProcess>();
        ffprobeProcess.ExecuteWithOutput().Returns("ffprobe version 4.4.1");

        externalProcessFactory.CreateExternalProcess(settings.FFMpegPath, "-version").Returns(ffmpegProcess);
        externalProcessFactory.CreateExternalProcess(settings.FFProbePath, "-version").Returns(ffprobeProcess);

        return externalProcessFactory;
    }
}
