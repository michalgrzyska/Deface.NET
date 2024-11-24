using Deface.NET.System;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class FFMpegVideoReader : IDisposable
{
    private readonly Action<FrameInfo> _frameProcess;
    private readonly ExternalProcess _ffmpegProcess;
    private readonly string _videoFilePath;
    private readonly VideoInfo _videoInfo = default!;

    private byte[] _buffer = [];
    private byte[] _rolloverBuffer = [];
    private int _totalBytesRead = 0;
    private int _frameSize = 0;

    public FFMpegVideoReader(string videoFilePath, Action<FrameInfo> frameProcess, string ffmpegPath, VideoInfo videoInfo)
    {
        _frameProcess = frameProcess;
        _videoFilePath = videoFilePath;
        _ffmpegProcess = GetFfmpegProcess(ffmpegPath);
        _videoInfo = videoInfo;
    }

    public void ProcessVideo()
    {
        SetupFields();

        _ffmpegProcess.Start();

        ProcessStream();
    }

    public void Dispose() => _ffmpegProcess?.Dispose();

    private void ProcessStream()
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
                ProcessFrame(i);
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

    private void ProcessFrame(int i)
    {
        byte[] frameData = new byte[_frameSize];
        Array.Copy(_buffer, 0, frameData, 0, _frameSize);

        FrameInfo frameInfo = new(frameData, i, _videoInfo.TotalFrames, _videoInfo.Width, _videoInfo.Height);
        _frameProcess(frameInfo);

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
