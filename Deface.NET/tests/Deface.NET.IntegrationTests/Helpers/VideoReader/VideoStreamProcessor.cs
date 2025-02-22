﻿namespace Deface.NET.IntegrationTests.Helpers.VideoReader;

internal class VideoStreamProcessor
{
    private readonly int _frameSize;

    private readonly byte[] _buffer = [];
    private readonly byte[] _rolloverBuffer = [];

    public const int ChannelsCount = 4;

    private int _totalBytesRead = 0;

    public VideoStreamProcessor(int width, int height)
    {
        _frameSize = width * height * ChannelsCount;

        _buffer = new byte[_frameSize];
        _rolloverBuffer = new byte[_frameSize];
    }

    public List<byte[]> ReadAllFrames(Stream stream)
    {
        List<byte[]> frames = [];

        while (true)
        {
            var bytesRead = stream.Read(_buffer, _totalBytesRead, _frameSize - _totalBytesRead);

            if (bytesRead == 0)
            {
                break;
            }

            _totalBytesRead += bytesRead;

            while (_totalBytesRead >= _frameSize)
            {
                var frame = GetFrame();
                frames.Add(frame);
            }
        }

        return frames;
    }

    private byte[] GetFrame()
    {
        var frame = new byte[_frameSize];
        Array.Copy(_buffer, 0, frame, 0, _frameSize);

        int excessBytes = _totalBytesRead - _frameSize;

        if (excessBytes > 0)
        {
            Array.Copy(_buffer, _frameSize, _rolloverBuffer, 0, excessBytes);
        }

        Array.Copy(_rolloverBuffer, 0, _buffer, 0, excessBytes);
        _totalBytesRead = excessBytes;

        return frame;
    }
}
