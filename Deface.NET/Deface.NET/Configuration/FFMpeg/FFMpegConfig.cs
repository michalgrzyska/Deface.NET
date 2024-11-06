using Deface.NET.Utils;

namespace Deface.NET;

/// <summary>
/// Configuration of FFMpeg and FFProbe.
/// </summary>
public class FFMpegConfig
{
    private readonly Platform _platform;

    /// <summary>
    /// FFMpeg and FFProbe configuration for Windows platform.
    /// </summary>
    public FFMpegPlatformConfig Windows { get; set; } = new();

    /// <summary>
    /// FFMpeg and FFProbe configuration for Linux platform.
    /// </summary>
    public FFMpegPlatformConfig Linux { get; set; } = new();

    internal FFMpegConfig(Platform platform)
    {
        _platform = platform;
    }

    internal FFMpegPlatformConfig GetCurrentConfig()
    {
        return _platform switch
        {
            Platform.Windows => Windows,
            Platform.Linux => Linux,
            _ => throw new NotImplementedException()
        };
    }
}