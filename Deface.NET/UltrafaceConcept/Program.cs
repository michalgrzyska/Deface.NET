using Deface.NET.TestResources;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        string videoFilePath = TestResources.Video_Short_HD_1280_720_24fps;
        ReadVideoFrames(videoFilePath);
    }

    static void ReadVideoFrames(string videoFilePath)
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
                ProcessFrame(frameData, width, height, i);

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
        var pixelFormat = PixelFormat.Format24bppRgb;
        int bytesPerPixel = Image.GetPixelFormatSize(pixelFormat) / 8;
        int stride = width * bytesPerPixel;
        byte[] rgbData = ConvertBgrToRgb(frameData);

        var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(rgbData, 0);

        var bitmap = new Bitmap(width, height, stride, pixelFormat, ptr);
        bitmap.Save("images/test" + i + ".png");
    }

    static byte[] ConvertBgrToRgb(byte[] bgrData)
    {
        byte[] rgbData = new byte[bgrData.Length];

        for (int i = 0; i < bgrData.Length; i += 3)
        {
            rgbData[i] = bgrData[i + 2];
            rgbData[i + 1] = bgrData[i + 1];
            rgbData[i + 2] = bgrData[i]; 
        }

        return rgbData;
    }
}
