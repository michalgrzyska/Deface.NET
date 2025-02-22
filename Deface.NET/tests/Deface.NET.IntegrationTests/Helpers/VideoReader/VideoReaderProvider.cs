using SkiaSharp;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Deface.NET.IntegrationTests.Helpers.VideoReader;

internal class VideoReaderProvider
(
    string ffmpegPath = "ffmpeg",
    string ffprobePath = "ffprobe"
)
{
    private readonly string _ffmpegPath = ffmpegPath;
    private readonly string _ffprobePath = ffprobePath;

    public VideoReadResult ReadVideo(string videoFilePath)
    {
        var videoInfo = GetVideoInfo(videoFilePath);

        var frames = GetVideoFrames(videoFilePath, videoInfo.Width, videoInfo.Height);

        var bitMapFrames = frames
            .Select(byteFrame => CreateBitmapFromBgra(byteFrame, videoInfo.Width, videoInfo.Height))
            .ToList();

        return new(bitMapFrames, videoInfo);
    }

    private List<byte[]> GetVideoFrames(string videoFilePath, int width, int height)
    {
        using var process = GetFfmpegProcess(videoFilePath);
        process.Start();

        VideoStreamProcessor processor = new(width, height);
        return processor.ReadAllFrames(process.OutputStream);
    }

    private VideoInfoStreamOutput GetVideoInfo(string videoFilePath)
    {
        using var ffprobe = GetFFProbeProcess(videoFilePath);

        var processOutput = ffprobe.ExecuteWithOutput();
        var output = JsonSerializer.Deserialize<VideoInfoOutput>(processOutput)!;

        var stream = output.Streams.FirstOrDefault()
            ?? throw new InvalidOperationException("Video has no streams."); ;

        return new()
        {
            Width = stream.Width,
            Height = stream.Height,
        };
    }

    private SKBitmap CreateBitmapFromBgra(byte[] bgraData, int width, int height)
    {
        SKBitmap bitmap = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        var pixelPtr = bitmap.GetPixels();

        Marshal.Copy(bgraData, 0, pixelPtr, bgraData.Length);

        return bitmap;
    }

    private ExternalProcess GetFfmpegProcess(string videoPath)
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

        return new ExternalProcess(_ffmpegPath, args, false);
    }

    private ExternalProcess GetFFProbeProcess(string filePath)
    {
        var args = string.Join(" ",
        [
            "-v", "error",
            "-select_streams", "v:0",
            "-show_entries", "stream=width,height",
            "-of", "json",
            $"\"{filePath}\""
        ]);

        return new ExternalProcess(_ffprobePath, args, false);
    }
}