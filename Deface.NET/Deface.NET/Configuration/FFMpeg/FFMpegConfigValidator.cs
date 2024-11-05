using Deface.NET.Utils;

namespace Deface.NET.Configuration.FFMpeg;

internal static class FFMpegConfigValidator
{
    public static void Validate(FFMpegConfig? config, string nameOfConfig)
    {
        ValidateConfig(config, nameOfConfig);

        var (platformConfig, currentPlatform) = GetConfigForCurrentPlatform(config!);

        ValidatePlatformConfig(platformConfig, currentPlatform.ToString());
        ValidateFFFile(platformConfig.FFMpegPath, nameof(platformConfig.FFMpegPath), currentPlatform.ToString());
        ValidateFFFile(platformConfig.FFProbePath, nameof(platformConfig.FFProbePath), currentPlatform.ToString());
    }

    private static void ValidateFFFile(string path, string nameOfProp, string prefix)
    {
        ValidationHelper.MustNotBeNullOrWhiteSpace(path, nameOfProp, prefix);
        ValidationHelper.FileMustExist(path, nameOfProp, prefix);
    }

    private static void ValidatePlatformConfig(FFMpegPlatformConfig platformConfig, string platformConfigPropName)
    {
        ValidationHelper.MustNotBeNull(platformConfig, platformConfigPropName);
    }

    private static void ValidateConfig(FFMpegConfig? config, string nameOfConfig)
    {
        ValidationHelper.MustNotBeNull(config!, nameOfConfig);
    }

    private static (FFMpegPlatformConfig, Platform) GetConfigForCurrentPlatform(FFMpegConfig config)
    {
        var currentPlatform = PlatformChecker.GetPlatform();

        var plaformConfig = currentPlatform switch
        {
            Platform.Windows => config.Windows,
            Platform.Linux => config.Linux,
            _ => throw new NotSupportedException("Platform is not supported.")
        };

        return (plaformConfig, currentPlatform);
    }
}
