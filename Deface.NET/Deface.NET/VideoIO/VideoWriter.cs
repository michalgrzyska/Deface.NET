using Deface.NET;
using System.Globalization;

internal class VideoWriter
{
    public void WriteVideo(
        List<byte[]> bitmaps,
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
            ffmpegInput.Write(bitmap, 0, bitmap.Length);
        }
    }
}
