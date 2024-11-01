using SkiaSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Deface.NET.VideoIO;

internal class VideoReader
{
    public VideoReader()
    {
        
    }

    public async Task Start(string videoFilePath)
    {
        VideoInfo videoInfo = await VideoInfo.GetInfo(videoFilePath);

        string ffmpegPath = "ffmpeg.exe";
        string arguments = $"-i \"{videoFilePath}\" -f image2pipe -pix_fmt rgb24 -vcodec rawvideo -";

        using Process ffmpeg = new();

        ffmpeg.StartInfo.FileName = ffmpegPath;
        ffmpeg.StartInfo.Arguments = arguments;
        ffmpeg.StartInfo.UseShellExecute = false;
        ffmpeg.StartInfo.RedirectStandardOutput = true;
        ffmpeg.StartInfo.CreateNoWindow = true;

        ffmpeg.Start();

        int frameSize = videoInfo.Width * videoInfo.Height * 3;
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

                byte[] rgbaData = ConvertBgrToRgba(frameData, videoInfo);
                SKBitmap bitmap = GetBitmapFromBytes(rgbaData, videoInfo);

                // fn!

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

    private static byte[] ConvertBgrToRgba(byte[] bgr, VideoInfo videoInfo)
    {
        byte[] rgbaData = new byte[videoInfo.Width * videoInfo.Height * 4];

        for (int j = 0; j < videoInfo.Width * videoInfo.Height; j++)
        {
            int b = bgr[j * 3];
            int g = bgr[j * 3 + 1];
            int r = bgr[j * 3 + 2];

            rgbaData[j * 4] = (byte)r;
            rgbaData[j * 4 + 1] = (byte)g;
            rgbaData[j * 4 + 2] = (byte)b;
            rgbaData[j * 4 + 3] = 255;
        }

        return rgbaData;
    }

    private static SKBitmap GetBitmapFromBytes(byte[] bytes, VideoInfo videoInfo) 
    {
        SKBitmap bitmap = new(videoInfo.Width, videoInfo.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        nint pixelPointer = handle.AddrOfPinnedObject();

        bitmap.InstallPixels(new SKImageInfo(videoInfo.Width, videoInfo.Height, SKColorType.Bgra8888), pixelPointer, videoInfo.Width * 4);
        return bitmap;
    }
}
