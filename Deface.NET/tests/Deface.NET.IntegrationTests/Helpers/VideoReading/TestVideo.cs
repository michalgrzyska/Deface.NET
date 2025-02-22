using Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading;

internal class TestVideo
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TotalFrames { get; private set; }
    public long Size { get; private set; }
    public float FrameRate { get; private set; }
    public List<TestFrame> Frames { get; private set; }

    private TestVideo(FFProbeVideoInfo info, FFMpegVideo video)
    {
        Width = info.Width;
        Height = info.Height;
        TotalFrames = info.TotalFrames;
        Size = info.Size;
        FrameRate = info.FrameRate;
        Frames = video.Frames;
    }

    public static async Task<TestVideo> Get(string videoPath)
    {
        var ffprobeInfo = await FFProbeVideoInfo.Get(videoPath);
        var ffmpegVideo = await FFMpegVideo.Get(videoPath, ffprobeInfo.Width, ffprobeInfo.Height);

        return new(ffprobeInfo, ffmpegVideo);
    }
}
