using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class FFMpegVideoReader : IDisposable
{
    private readonly Action<FrameInfo> _frameProcess;
    private readonly ExternalProcess _ffmpegProcess;
    private readonly VideoInfo _videoInfo = default!;

    public FFMpegVideoReader(Action<FrameInfo> frameProcess, string ffmpegPath, VideoInfo videoInfo)
    {
        _frameProcess = frameProcess;
        _videoInfo = videoInfo;

        _ffmpegProcess = GetFfmpegProcess(ffmpegPath);
    }

    public void ProcessVideo()
    {
        _ffmpegProcess.Start();

        VideoStreamProcessor processor = new(_videoInfo);
        processor.Process(_ffmpegProcess.OutputStream, _frameProcess);
    }

    public void Dispose() => _ffmpegProcess?.Dispose();


    private ExternalProcess GetFfmpegProcess(string ffmpegPath)
    {
        string args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-i", $"\"{_videoInfo.Path}\"",
            "-f", "image2pipe",
            "-pix_fmt", "rgb24",
            "-vcodec", "rawvideo",
            "-"
        ]);

        return new ExternalProcess(ffmpegPath, args);
    }
}
