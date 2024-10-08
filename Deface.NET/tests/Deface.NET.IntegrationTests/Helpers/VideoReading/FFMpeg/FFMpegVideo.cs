using Deface.NET.IntegrationTests.Config;
using System.Diagnostics;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg;

internal class FFMpegVideo
{
    public List<TestFrame> Frames { get; private set; }

    private FFMpegVideo(List<TestFrame> frames)
    {
        Frames = frames;
    }

    public static async Task<FFMpegVideo> Get(string filePath, int width, int height)
    {
        using var process = GetFfmpegProcess(filePath);
        process.Start();

        VideoStreamProcessor processor = new(width, height);
        var bytesFrames = processor.ReadAllFrames(process.StandardOutput.BaseStream);

        await process.WaitForExitAsync();

        var frames = bytesFrames.Select(x => new TestFrame(x, width, height)).ToList();

        return new(frames);
    }

    private static Process GetFfmpegProcess(string videoPath)
    {
        var args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-i", $"\"{videoPath}\"",
            "-f", "image2pipe",
            "-pix_fmt", "bgra",
            "-vcodec", "rawvideo",
            "-"
        ]);

        return new()
        {
            StartInfo = new()
            {
                FileName = ExecutablePath.FFMpeg,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            }
        };
    }
}
