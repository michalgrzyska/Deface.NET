namespace Deface.NET;

/// <summary>
/// Represents codecs available to encode a result video.
/// </summary>
public enum EncodingCodec
{
    /// <summary>
    /// VP9 is an open source codec. Videos will be saved as .webm.
    /// </summary>
    VP9,
    /// <summary>
    /// H264 is a commonly used commercial codec. Videos will be saved as .mp4.
    /// </summary>
    /// <remarks>
    /// Ensure you have a proper rights to use H264.
    /// </remarks>
    H264
}
