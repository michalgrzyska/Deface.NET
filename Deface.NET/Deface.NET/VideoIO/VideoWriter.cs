using Deface.NET;
using Deface.NET.Utils;
using Deface.NET.VideoIO;
using System.Globalization;

internal static class VideoWriter
{
    public static void WriteVideo(List<byte[]> frames, VideoInfo videoInfo, string outputPath, Settings settings)
    {
        using ExternalProcess ffmpegProcess = GetFfmpegProcess(videoInfo, outputPath, settings);
        ffmpegProcess.Start();

        using var ffmpegInput = ffmpegProcess.InputStream;

        foreach (var frame in frames)
        {
            ffmpegInput.Write(frame);
        }
    }

    private static ExternalProcess GetFfmpegProcess(VideoInfo videoInfo, string outputPath, Settings settings)
    {
        string ffmpegPath = settings.FFMpegConfig.GetCurrentConfig().FFMpegPath;

        string args = string.Join(" ",
        [
            "-y",
            "-f", "rawvideo",
            "-pixel_format", "rgb24",
            "-video_size", $"{videoInfo.Width}x{videoInfo.Height}",
            "-framerate",  videoInfo.AverageFps.ToString(CultureInfo.InvariantCulture),
            "-i", "-",
            "-c:v", "libx264",
            "-pix_fmt yuv420p",
            $"\"{outputPath}\""
        ]);

        return new ExternalProcess(ffmpegPath, args, redirectStandardInput: true);
    }
}
