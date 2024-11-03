namespace Deface.NET;

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
