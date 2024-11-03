using Deface.NET.Utils;

namespace Deface.NET;

/// <summary>
/// Configuration of FFMpeg and FFProbe.
/// </summary>
public class FFMpegConfig
{
    /// <summary>
    /// FFMpeg and FFProbe configuration for Windows platform.
    /// </summary>
    public FFMpegPlatformConfig Windows { get; set; } = new();

    /// <summary>
    /// FFMpeg and FFProbe configuration for Linux platform.
    /// </summary>
    public FFMpegPlatformConfig Linux { get; set; } = new();

    internal FFMpegPlatformConfig GetCurrentConfig()
    {
        var currentPlatform = PlatformChecker.GetPlatform();

        return currentPlatform switch
        {
            Platform.Windows => Windows,
            Platform.Linux => Linux,
            _ => throw new NotImplementedException()
        };
    }
}