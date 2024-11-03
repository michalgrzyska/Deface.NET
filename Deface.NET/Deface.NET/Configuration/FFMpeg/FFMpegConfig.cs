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

/// <summary>
/// Configuration of FFMpeg and FFProbe for selected platform.
/// </summary>
public class FFMpegPlatformConfig
{
    /// <summary>
    /// Path to an FFMpeg executable. Can be relative or absolute.
    /// </summary>
    public string FFMpegPath { get; set; } = string.Empty;

    /// <summary>
    /// Path to an FFProbe executable. Can be relative or absolute.
    /// </summary>
    public string FFProbePath { get; set; } = string.Empty;
}