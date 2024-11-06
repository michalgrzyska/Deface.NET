using Deface.NET.Configuration.FFMpeg;
using Deface.NET.Utils;
using FluentAssertions;

namespace Deface.NET.UnitTests.SettingsTests;

public class FFMpegConfigUnitTests
{
    private readonly TestSettingsProvider _settingsProvider;

    public FFMpegConfigUnitTests()
    {
        _settingsProvider = new();
    }

    [Fact]
    public void New_NoLinuxDataProvidedForLinux_ThrowsArgumentNullException()
    {
        FFMpegConfig config = new(Platform.Linux);

        var action = () => FFMpegConfigValidator.Validate(config, "Test");

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_NoWindowsDataProvidedForWindows_ThrowsArgumentNullException()
    {
        FFMpegConfig config = new(Platform.Windows);

        var action = () => FFMpegConfigValidator.Validate(config, "Test");

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_NullConfig_ThrowsArgumentNullException()
    {
        var action = () => FFMpegConfigValidator.Validate(null, "Test");

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_InvalidWindowsFFMpegPathProvided_ThrowsFileNotFoundException()
    {
        var settings = _settingsProvider.GetSettings(Platform.Windows);
        var configSection = settings.FFMpegConfig.GetCurrentConfig();

        configSection.FFMpegPath = Guid.NewGuid().ToString();

        var action = () => FFMpegConfigValidator.Validate(settings.FFMpegConfig, "Test");

        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void New_InvalidWindowsFFProbePathProvided_ThrowsFileNotFoundException()
    {
        var settings = _settingsProvider.GetSettings(Platform.Windows);
        var configSection = settings.FFMpegConfig.GetCurrentConfig();

        configSection.FFProbePath = Guid.NewGuid().ToString();

        var action = () => FFMpegConfigValidator.Validate(settings.FFMpegConfig, "Test");

        action.Should().Throw<FileNotFoundException>();
    }

    [Theory]
    [InlineData((int)Platform.Windows, nameof(FFMpegConfig.Windows))]
    [InlineData((int)Platform.Linux, nameof(FFMpegConfig.Linux))]
    public void GetCurrentConfig_ProperConfigGiven(int platformValue, string propName) // int passed due to Plaform being internal (must be public for normal pass)
    {
        var ffmpegTestValue = "ffmpegtest";
        var ffprobeTestValue = "ffprobetest";

        Platform platform = (Platform)platformValue;

        var settings = _settingsProvider.GetSettings(platform);
        var configSection = GetPropertyValue<FFMpegPlatformConfig>(settings.FFMpegConfig, propName);

        configSection.FFMpegPath = ffmpegTestValue;
        configSection.FFProbePath = ffprobeTestValue;

        var configSectionFromMethod = settings.FFMpegConfig.GetCurrentConfig();

        configSectionFromMethod.FFMpegPath.Should().Be(ffmpegTestValue);
        configSectionFromMethod.FFProbePath.Should().Be(ffprobeTestValue);
    }

    private static T GetPropertyValue<T>(object obj, string propertyName)
    {
        var propertyInfo = obj.GetType().GetProperty(propertyName);
        return (T)propertyInfo!.GetValue(obj)!;
    }
}
