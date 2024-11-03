using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

class VideoWriter
{
    public void CreateVideoFromBitmaps(
        List<SKBitmap> bitmaps,
        int width,
        int height,
        float fps,
        string outputPath)
    {
        // Convert FPS to string with dot as a decimal separator
        string fpsString = fps.ToString(CultureInfo.InvariantCulture);

        // Set up the ffmpeg arguments for raw RGB input
        var ffmpegArgs = $"-y -f rawvideo -pixel_format rgb24 -video_size {width}x{height} -framerate {fpsString} -i - -c:v libx264 -pix_fmt yuv420p \"{outputPath}\"";

        // Start the ffmpeg process
        using (var ffmpegProcess = new Process())
        {
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

                using (var ffmpegInput = ffmpegProcess.StandardInput.BaseStream)
                {
                    // Write each SKBitmap as raw RGB data to ffmpeg input
                    foreach (var bitmap in bitmaps)
                    {
                        // Ensure each bitmap has the correct dimensions
                        if (bitmap.Width != width || bitmap.Height != height)
                        {
                            throw new ArgumentException("Bitmap size does not match the specified width and height.");
                        }

                        // Extract RGB data from SKBitmap
                        byte[] rgbData = BitmapToRgb(bitmap);

                        // Ensure data is exactly the right size for FFmpeg
                        if (rgbData.Length != width * height * 3)
                        {
                            throw new InvalidOperationException("RGB data size mismatch. Check frame dimensions and pixel format.");
                        }

                        // Write frame to ffmpeg
                        ffmpegInput.Write(rgbData, 0, rgbData.Length);
                    }
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

    private static byte[] BitmapToRgb(SKBitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        int bytesPerPixel = 3; // RGB24 requires 3 bytes per pixel
        byte[] rgbData = new byte[width * height * bytesPerPixel];

        // Convert the SKBitmap to SKImage to access pixels directly in BGRA format
        using (var image = SKImage.FromBitmap(bitmap))
        using (var pixmap = image.PeekPixels())
        {
            if (pixmap == null)
                throw new Exception("Failed to access pixels from SKImage.");

            // Check for BGRA8888 format and convert if needed
            if (pixmap.ColorType != SKColorType.Bgra8888)
                throw new Exception("The pixel format is not BGRA8888, which is expected.");

            // Allocate a temporary buffer to hold BGRA data
            byte[] bgraData = new byte[width * height * 4]; // BGRA8888 has 4 bytes per pixel

            // Pin the bgraData array to get a stable pointer
            var handle = GCHandle.Alloc(bgraData, GCHandleType.Pinned);
            try
            {
                IntPtr bgraDataPtr = handle.AddrOfPinnedObject();

                // Read BGRA data into bgraData array
                if (!pixmap.ReadPixels(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), bgraDataPtr, width * 4))
                {
                    throw new Exception("Failed to read BGRA pixels into data array.");
                }

                // Convert BGRA to RGB by discarding the alpha and reordering bytes
                for (int i = 0, j = 0; i < bgraData.Length; i += 4, j += 3)
                {
                    // BGRA -> RGB (ignore the alpha channel)
                    rgbData[j] = bgraData[i + 2];     // Red
                    rgbData[j + 1] = bgraData[i + 1]; // Green
                    rgbData[j + 2] = bgraData[i];     // Blue
                }
            }
            finally
            {
                // Free the pinned handle
                handle.Free();
            }
        }

        return rgbData;
    }
}
