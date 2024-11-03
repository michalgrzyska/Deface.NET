using SkiaSharp;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

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

                byte[] rgbData = BitmapToRgb(bitmap);

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

    private static byte[] BitmapToRgb(SKBitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        int bytesPerPixel = 3;
        byte[] rgbData = new byte[width * height * bytesPerPixel];

        using SKImage image = SKImage.FromBitmap(bitmap);
        using SKPixmap pixmap = image.PeekPixels();
        
        if (pixmap == null)
            throw new Exception("Failed to access pixels from SKImage.");

        if (pixmap.ColorType != SKColorType.Bgra8888)
            throw new Exception("The pixel format is not BGRA8888, which is expected.");

        byte[] bgraData = new byte[width * height * 4]; 

        var handle = GCHandle.Alloc(bgraData, GCHandleType.Pinned);

        try
        {
            IntPtr bgraDataPtr = handle.AddrOfPinnedObject();

            if (!pixmap.ReadPixels(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), bgraDataPtr, width * 4))
            {
                throw new Exception("Failed to read BGRA pixels into data array.");
            }

            for (int i = 0, j = 0; i < bgraData.Length; i += 4, j += 3)
            {
                rgbData[j] = bgraData[i + 2];
                rgbData[j + 1] = bgraData[i + 1];
                rgbData[j + 2] = bgraData[i];
            }
        }
        finally
        {
            handle.Free();
        }
        
        return rgbData;
    }
}
