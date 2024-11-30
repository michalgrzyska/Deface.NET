using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class VideoStreamProcessor
{
    private readonly VideoInfo _videoInfo;
    private readonly int _frameSize;

    private readonly byte[] _buffer = [];
    private readonly byte[] _rolloverBuffer = [];

    private int _totalBytesRead = 0;

    public VideoStreamProcessor(VideoInfo videoInfo)
    {
        _videoInfo = videoInfo;
        _frameSize = videoInfo.Width * videoInfo.Height * 3;

        _buffer = new byte[_frameSize];
        _rolloverBuffer = new byte[_frameSize];
    }

    public void Process(Stream stream, Action<FrameInfo> action)
    {
        int i = 0;

        while (true)
        {
            int bytesRead = stream.Read(_buffer, _totalBytesRead, _frameSize - _totalBytesRead);

            if (bytesRead == 0)
            {
                break;
            }

            _totalBytesRead += bytesRead;

            while (_totalBytesRead >= _frameSize)
            {
                ProcessFrame(i, action);
                i++;
            }
        }
    }

    private void ProcessFrame(int i, Action<FrameInfo> action)
    {
        byte[] frameData = new byte[_frameSize];
        Array.Copy(_buffer, 0, frameData, 0, _frameSize);

        FrameInfo frameInfo = new(frameData, i, _videoInfo.TotalFrames, _videoInfo.Width, _videoInfo.Height);
        action(frameInfo);

        int excessBytes = _totalBytesRead - _frameSize;

        if (excessBytes > 0)
        {
            Array.Copy(_buffer, _frameSize, _rolloverBuffer, 0, excessBytes);
        }

        Array.Copy(_rolloverBuffer, 0, _buffer, 0, excessBytes);
        _totalBytesRead = excessBytes;
    }
}
