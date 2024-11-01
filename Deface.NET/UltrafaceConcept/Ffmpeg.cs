using Deface.NET.TestResources;
using SkiaSharp;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using UltrafaceConcept;

class Ffmpeg
{
    Ultraface ultraface = new();

    public void Main()
    {
        string videoFilePath = TestResources.Video_Short_HD_1280_720_24fps;
        ReadVideoFrames(videoFilePath);
    }

    void ReadVideoFrames(string videoFilePath)
    {
        string ffmpegPath = "ffmpeg.exe";
        string arguments = $"-i \"{videoFilePath}\" -f image2pipe -pix_fmt rgb24 -vcodec rawvideo -";

        using Process ffmpeg = new();

        ffmpeg.StartInfo.FileName = ffmpegPath;
        ffmpeg.StartInfo.Arguments = arguments;
        ffmpeg.StartInfo.UseShellExecute = false;
        ffmpeg.StartInfo.RedirectStandardOutput = true;
        ffmpeg.StartInfo.CreateNoWindow = true;

        ffmpeg.Start();

        int width = 1280;
        int height = 720;
        int frameSize = width * height * 3;
        byte[] buffer = new byte[frameSize];
        int totalBytesRead = 0;

        byte[] rolloverBuffer = new byte[frameSize];
        int i = 0;


        while (true)
        {
            int bytesRead = ffmpeg.StandardOutput.BaseStream.Read(buffer, totalBytesRead, frameSize - totalBytesRead);

            if (bytesRead == 0)
            {
                break;
            }

            totalBytesRead += bytesRead;

            while (totalBytesRead >= frameSize)
            {
                byte[] frameData = new byte[frameSize];

                Array.Copy(buffer, 0, frameData, 0, frameSize);
                //ProcessFrame(frameData, width, height, i);

                byte[] rgbaData = new byte[width * height * 4];

                for (int j = 0; j < width * height; j++)
                {
                    int b = frameData[j * 3];
                    int g = frameData[j * 3 + 1];
                    int r = frameData[j * 3 + 2];

                    rgbaData[j * 4] = (byte)r;
                    rgbaData[j * 4 + 1] = (byte)g;
                    rgbaData[j * 4 + 2] = (byte)b;
                    rgbaData[j * 4 + 3] = 255;
                }

                using var bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
                GCHandle handle = GCHandle.Alloc(rgbaData, GCHandleType.Pinned);
                nint pixelPointer = handle.AddrOfPinnedObject();
                bitmap.InstallPixels(new SKImageInfo(width, height, SKColorType.Bgra8888), pixelPointer, width * 4);

                var faces = ultraface.Process(bitmap);
                Console.WriteLine($"Frame {i}: {faces.Count} faces");
                int excessBytes = totalBytesRead - frameSize;

                if (excessBytes > 0)
                {
                    Array.Copy(buffer, frameSize, rolloverBuffer, 0, excessBytes);
                }

                Array.Copy(rolloverBuffer, 0, buffer, 0, excessBytes);
                totalBytesRead = excessBytes;

                i++;
            }
        }
    }

    static void ProcessFrame(byte[] frameData, int width, int height, int i)
    {
        if (frameData == null || frameData.Length != width * height * 3)
        {
            throw new ArgumentException("Invalid frame data.");
        }

        byte[] rgbaData = new byte[width * height * 4];

        for (int j = 0; j < width * height; j++)
        {
            int b = frameData[j * 3];
            int g = frameData[j * 3 + 1];
            int r = frameData[j * 3 + 2];

            rgbaData[j * 4] = (byte)r;
            rgbaData[j * 4 + 1] = (byte)g;
            rgbaData[j * 4 + 2] = (byte)b;
            rgbaData[j * 4 + 3] = 255;
        }

        using (var bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul))
        {
            GCHandle handle = GCHandle.Alloc(rgbaData, GCHandleType.Pinned);

            try
            {
                nint pixelPointer = handle.AddrOfPinnedObject();
                bitmap.InstallPixels(new SKImageInfo(width, height, SKColorType.Bgra8888), pixelPointer, width * 4);
            }
            finally
            {
                handle.Free();
            }

            string directoryPath = "images";
            string filePath = Path.Combine(directoryPath, $"{i}.png");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                data.SaveTo(stream);
            }
        }
    }
}
