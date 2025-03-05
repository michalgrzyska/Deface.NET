using Deface.NET.Common;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Helpers;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;
using System.Globalization;

namespace Deface.NET.VideoIO;

internal class VideoWriter(IScopedSettingsProvider settingsProvider, IExternalProcessFactory externalProcessFactory) : IVideoWriter
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public void WriteVideo(List<Frame> frames, VideoInfo videoInfo, string outputPath)
    {
        using var ffmpegProcess = GetFfmpegProcess(videoInfo, outputPath);
        ffmpegProcess.Start();

        using var ffmpegInput = ffmpegProcess.InputStream;
        var frameIndex = 0;

        try
        {
            foreach (var frame in frames)
            {
                ffmpegInput.Write(frame.Bytes);
                frameIndex++;
            }
        }
        catch (IOException)
        {
            if (frameIndex == 0 || frameIndex == 1)
            {
                throw new InvalidOperationException(ExceptionMessages.ErrorWhileWritingFrames);
            }
        }

        ffmpegInput.Close();
        ffmpegProcess.WaitForExit();
    }

    private IExternalProcess GetFfmpegProcess(VideoInfo videoInfo, string outputPath)
    {
        var ffmpegPath = _settings.FFMpegPath;
        var codec = CodecHelper.GetCodecName(_settings.EncodingCodec);

        var args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-y",
            "-f", "rawvideo",
            "-pixel_format", "bgra",
            "-video_size", $"{videoInfo.Width}x{videoInfo.Height}",
            "-framerate", videoInfo.AverageFps.ToString(CultureInfo.InvariantCulture),
            "-i", "-",
            "-c:v", codec, "-speed 8",
            "-pix_fmt", "yuv420p",
            $"\"{outputPath}\""
        ]);

        return _externalProcessFactory.CreateExternalProcess(ffmpegPath, args, redirectStandardInput: true);
    }
}
