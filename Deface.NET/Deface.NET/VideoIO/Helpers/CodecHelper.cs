using Deface.NET.VideoIO.Constants;

namespace Deface.NET.VideoIO.Helpers;

internal static class CodecHelper
{
    private static readonly Dictionary<EncodingCodec, string> _codecsNames = new()
    {
        { EncodingCodec.VP9, CodecNames.VP9 },
        { EncodingCodec.H264, CodecNames.H264 },
    };

    private static readonly Dictionary<EncodingCodec, string> _codecsExtensions = new()
    {
        { EncodingCodec.VP9, VideoExtensions.WebM },
        { EncodingCodec.H264, VideoExtensions.Mp4 },
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
