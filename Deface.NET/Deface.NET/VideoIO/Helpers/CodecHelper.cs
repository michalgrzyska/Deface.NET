namespace Deface.NET.VideoIO.Helpers;

internal static class CodecHelper
{
    private static readonly Dictionary<EncodingCodec, string> _codecsNames = new()
    {
        { EncodingCodec.VP9, "libvpx-vp9" },
        { EncodingCodec.H264, "libx264" },
    };

    private static readonly Dictionary<EncodingCodec, string> _codecsExtensions = new()
    {
        { EncodingCodec.VP9, "webm" },
        { EncodingCodec.H264, "mp4" },
    };

    public static string GetCodecName(EncodingCodec codec)
    {
        return _codecsNames[codec];
    }

    public static string GetCodecExtension(EncodingCodec codec)
    {
        return _codecsExtensions[codec];
    }
}
