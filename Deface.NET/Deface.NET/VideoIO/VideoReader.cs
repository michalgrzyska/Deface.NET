using Deface.NET.Graphics;
using Deface.NET.Utils;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class VideoReader : IDisposable
{
    private readonly Func<Frame, int, int, Task> _frameProcess;
    private readonly ExternalProcess _ffmpegProcess;
    private readonly string _videoFilePath;
    private readonly VideoInfo _videoInfo = default!;

    private byte[] _buffer = [];
    private byte[] _rolloverBuffer = [];
    private int _totalBytesRead = 0;
    private int _frameSize = 0;

    public VideoReader(string videoFilePath, Func<Frame, int, int, Task> frameProcess, string ffmpegPath, VideoInfo videoInfo)
    {
        _frameProcess = frameProcess;
        _videoFilePath = videoFilePath;
        _ffmpegProcess = GetFfmpegProcess(ffmpegPath);
        _videoInfo = videoInfo;
    }

    public async Task ProcessVideo()
    {
        SetupFields();

        _ffmpegProcess.Start();

        await ProcessStream();
    }

    public void Dispose() => _ffmpegProcess?.Dispose();

    private async Task ProcessStream()
    {
        int i = 0;

        while (true)
        {
            int bytesRead = _ffmpegProcess.OutputStream.Read(_buffer, _totalBytesRead, _frameSize - _totalBytesRead);

            if (bytesRead == 0)
            {
                break;
            }

            _totalBytesRead += bytesRead;

            while (_totalBytesRead >= _frameSize)
            {
                await ProcessFrame(i);
                i++;
            }
        }
    }

    private void SetupFields()
    {
        _frameSize = _videoInfo.Width * _videoInfo.Height * 3;
        _buffer = new byte[_frameSize];
        _rolloverBuffer = new byte[_frameSize];
    }

    private async Task ProcessFrame(int i)
    {
        byte[] frameData = new byte[_frameSize];
        Array.Copy(_buffer, 0, frameData, 0, _frameSize);

        Frame frame = new(frameData, _videoInfo.Width, _videoInfo.Height);
        await _frameProcess(frame, i, _videoInfo.TotalFrames);

        int excessBytes = _totalBytesRead - _frameSize;

        if (excessBytes > 0)
        {
            Array.Copy(_buffer, _frameSize, _rolloverBuffer, 0, excessBytes);
        }

        Array.Copy(_rolloverBuffer, 0, _buffer, 0, excessBytes);
        _totalBytesRead = excessBytes;
    }

    private ExternalProcess GetFfmpegProcess(string ffmpegPath)
    {
        string args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-i", $"\"{_videoFilePath}\"",
            "-f", "image2pipe",
            "-pix_fmt", "rgb24",
            "-vcodec", "rawvideo",
            "-"
        ]);

        return new ExternalProcess(ffmpegPath, args);
    }
}
