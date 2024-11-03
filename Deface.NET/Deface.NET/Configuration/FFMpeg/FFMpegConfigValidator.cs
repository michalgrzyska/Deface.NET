using Deface.NET.Utils;

namespace Deface.NET.Configuration.FFMpeg;

internal static class FFMpegConfigValidator
{
    public static void Validate(FFMpegConfig? config, string nameOfConfig)
    {
        if (config is null)
        {
            throw new InvalidDataException($"{nameOfConfig} must at least 1.0.");
        }

        var (platformConfig, currentPlatform) = GetConfigForCurrentPlatform(config);

        if (platformConfig is null)
        {
            throw new InvalidDataException($"Config for {currentPlatform} must not be null.");
        }

        if (string.IsNullOrEmpty(platformConfig.FFMpegPath))
        {
            throw new InvalidDataException($"{nameof(platformConfig.FFMpegPath)} for {currentPlatform} must not be null/empty.");
        }

        if (string.IsNullOrEmpty(platformConfig.FFProbePath))
        {
            throw new InvalidDataException($"{nameof(platformConfig.FFProbePath)} for {currentPlatform} must not be null/empty.");
        }
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
