using Deface.NET.Graphics;
using SkiaSharp;
using System.Diagnostics;
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

        using var ffmpegProcess = new Process();

        ffmpegProcess.StartInfo.FileName = "ffmpeg.exe";
        ffmpegProcess.StartInfo.Arguments = ffmpegArgs;
        ffmpegProcess.StartInfo.UseShellExecute = false;
        ffmpegProcess.StartInfo.RedirectStandardInput = true;
        ffmpegProcess.StartInfo.RedirectStandardOutput = true;
        ffmpegProcess.StartInfo.RedirectStandardError = true;
        ffmpegProcess.StartInfo.CreateNoWindow = true;

        try
        {
            ffmpegProcess.Start();

            using var ffmpegInput = ffmpegProcess.StandardInput.BaseStream;

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

            // Wait for the ffmpeg process to finish
            //ffmpegProcess.WaitForExit();

            //// Check the exit code for success
            //if (ffmpegProcess.ExitCode != 0)
            //{
            //    string ffmpegError = ffmpegProcess.StandardError.ReadToEnd();
            //    throw new Exception($"FFmpeg exited with code {ffmpegProcess.ExitCode}. Error: {ffmpegError}");
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
