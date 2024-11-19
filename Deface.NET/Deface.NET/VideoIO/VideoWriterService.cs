using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics;
using Deface.NET.Utils;
using Deface.NET.VideoIO.Models;
using System.Globalization;

namespace Deface.NET.VideoIO;

internal class VideoWriterService(IScopedSettingsProvider settingsProvider)
{
    private readonly Settings _settings = settingsProvider.Settings;

    public void WriteVideo(List<Frame> frames, VideoInfo videoInfo, string outputPath)
    {
        using ExternalProcess ffmpegProcess = GetFfmpegProcess(videoInfo, outputPath);
        ffmpegProcess.Start();

        using var ffmpegInput = ffmpegProcess.InputStream;

        foreach (var frame in frames) using (frame)
            {
                var bytes = frame.ToByteArray();
                ffmpegInput.Write(bytes);
            }
    }

    private ExternalProcess GetFfmpegProcess(VideoInfo videoInfo, string outputPath)
    {
        string ffmpegPath = _settings.FFMpegPath;

        string args = string.Join(" ",
        [
            "-loglevel", "panic",
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
