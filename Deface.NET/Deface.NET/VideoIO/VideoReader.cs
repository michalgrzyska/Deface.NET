using SkiaSharp;

namespace Deface.NET.VideoIO;

internal class VideoReader : IDisposable
{
    private readonly Func<SKBitmap, int, Task> _frameProcess;
    private readonly ExternalProcess _ffmpegProcess;
    private readonly string _videoFilePath;

    private VideoInfo _videoInfo = default!;
    private byte[] _buffer = [];
    private byte[] _rolloverBuffer = [];
    private int _totalBytesRead = 0;
    private int _frameSize = 0;

    public VideoReader(string videoFilePath, Func<SKBitmap, int, Task> frameProcess)
    {
        _frameProcess = frameProcess;
        _videoFilePath = videoFilePath;

        _ffmpegProcess = GetFfmpegProcess();
    }

    public async Task Start()
    {
        await SetupFields();

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

    private async Task SetupFields()
    {
        _videoInfo = await VideoInfo.GetInfo(_videoFilePath);

        _frameSize = _videoInfo.Width * _videoInfo.Height * 3;
        _buffer = new byte[_frameSize];
        _rolloverBuffer = new byte[_frameSize];
    }

    private async Task ProcessFrame(int i)
    {
        byte[] frameData = new byte[_frameSize];
        Array.Copy(_buffer, 0, frameData, 0, _frameSize);

        byte[] rgbaData = GraphicsHelper.ConvertBgrToRgba(frameData, _videoInfo.Width, _videoInfo.Height);
        SKBitmap bitmap = GraphicsHelper.GetBitmapFromBytes(rgbaData, _videoInfo.Width, _videoInfo.Height);

        await _frameProcess(bitmap, i);

        int excessBytes = _totalBytesRead - _frameSize;

        if (excessBytes > 0)
        {
            Array.Copy(_buffer, _frameSize, _rolloverBuffer, 0, excessBytes);
        }

        Array.Copy(_rolloverBuffer, 0, _buffer, 0, excessBytes);
        _totalBytesRead = excessBytes;
    }

    private ExternalProcess GetFfmpegProcess()
    {
        string file = "ffmpeg.exe";
        string parameters = $"-i \"{_videoFilePath}\" -f image2pipe -pix_fmt rgb24 -vcodec rawvideo -";

        return new(file, parameters);
    }
}
