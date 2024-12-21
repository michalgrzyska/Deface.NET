namespace Deface.NET.UnitTests.VideoIO.EncoderChecker;

internal static class VideoCheckerUnitTestsContants
{
    public const string ValidOutput = @"
        Encoders:
        V..... = Video
        A..... = Audio
        S..... = Subtitle
        .F.... = Frame-level multithreading
        ..S... = Slice-level multithreading
        ...X.. = Codec is experimental
        ....B. = Supports draw_horiz_band
        .....D = Supports direct rendering method 1
        ------
        V....D libx264              libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (codec h264)
        V....D libx264rgb           libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
        V....D vp8_vaapi            VP8 (VAAPI) (codec vp8)
        V....D libvpx-vp9           libvpx VP9 (codec vp9)
        V....D vp9_vaapi            VP9 (VAAPI) (codec vp9)";

    public const string NoVP9 = @"
        Encoders:
        V..... = Video
        A..... = Audio
        S..... = Subtitle
        .F.... = Frame-level multithreading
        ..S... = Slice-level multithreading
        ...X.. = Codec is experimental
        ....B. = Supports draw_horiz_band
        .....D = Supports direct rendering method 1
        ------
        V....D libx264              libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (codec h264)
        V....D libx264rgb           libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
        V....D vp8_vaapi            VP8 (VAAPI) (codec vp8)
        V....D vp9_vaapi            VP9 (VAAPI) (codec vp9)";

    public const string NoH264 = @"
        Encoders:
        V..... = Video
        A..... = Audio
        S..... = Subtitle
        .F.... = Frame-level multithreading
        ..S... = Slice-level multithreading
        ...X.. = Codec is experimental
        ....B. = Supports draw_horiz_band
        .....D = Supports direct rendering method 1
        ------
        V....D libx264rgb           libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
        V....D vp8_vaapi            VP8 (VAAPI) (codec vp8)
        V....D libvpx-vp9           libvpx VP9 (codec vp9)
        V....D vp9_vaapi            VP9 (VAAPI) (codec vp9)";

    public const string NoBoth = @"
        Encoders:
        V..... = Video
        A..... = Audio
        S..... = Subtitle
        .F.... = Frame-level multithreading
        ..S... = Slice-level multithreading
        ...X.. = Codec is experimental
        ....B. = Supports draw_horiz_band
        .....D = Supports direct rendering method 1
        ------
        V....D libx264rgb           libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
        V....D vp8_vaapi            VP8 (VAAPI) (codec vp8)
        V....D vp9_vaapi            VP9 (VAAPI) (codec vp9)";

    public const string NoEncodersPrepended = @"
        V....D libx264              libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 (codec h264)
        V....D libx264rgb           libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
        V....D vp8_vaapi            VP8 (VAAPI) (codec vp8)
        V....D libvpx-vp9           libvpx VP9 (codec vp9)
        V....D vp9_vaapi            VP9 (VAAPI) (codec vp9)";
}
