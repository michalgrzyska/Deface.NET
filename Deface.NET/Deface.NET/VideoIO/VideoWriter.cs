using Deface.NET;
using Deface.NET.Graphics;
using SkiaSharp;
using System.Globalization;

internal class VideoWriter
{
    public void CreateVideoFromBitmaps(
        List<SKBitmap> bitmaps,
        int width,
        int height,
        float fps,
        string outputPath)
    {
        string fpsString = fps.ToString(CultureInfo.InvariantCulture);

        var ffmpegArgs = $"-y -f rawvideo -pixel_format rgb24 -video_size {width}x{height} -framerate {fpsString} -i - -c:v libx264 -pix_fmt yuv420p \"{outputPath}\"";

        using ExternalProcess ffmpegProcess = new(
            "ffmpeg.exe",
            ffmpegArgs,
            redirectStandardInput: true
        );

        ffmpegProcess.Start();

        using var ffmpegInput = ffmpegProcess.InputStream;

        foreach (var bitmap in bitmaps)
        {
            if (bitmap.Width != width || bitmap.Height != height)
            {
                throw new ArgumentException("Bitmap size does not match the specified width and height.");
            }

            byte[] rgbData = GraphicsHelper.ConvertSKBitmapToRgbByteArray(bitmap);

            if (rgbData.Length != width * height * 3)
            {
                throw new InvalidOperationException("RGB data size mismatch. Check frame dimensions and pixel format.");
            }

            ffmpegInput.Write(rgbData, 0, rgbData.Length);
        }
    }
}
