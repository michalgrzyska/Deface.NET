using Deface.NET;
using Deface.NET.VideoIO;
using System.Globalization;

internal static class VideoWriter
{
    public static void WriteVideo(List<byte[]> frames, VideoInfo videoInfo, string outputPath)
    {
        using ExternalProcess ffmpegProcess = GetFfmpegProcess(videoInfo, outputPath);
        ffmpegProcess.Start();

        using var ffmpegInput = ffmpegProcess.InputStream;

        foreach (var frame in frames)
        {
            ffmpegInput.Write(frame);
        }
    }

    private static ExternalProcess GetFfmpegProcess(VideoInfo videoInfo, string outputPath)
    {
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

        return new ExternalProcess("ffmpeg.exe", args, redirectStandardInput: true);
    }
}
