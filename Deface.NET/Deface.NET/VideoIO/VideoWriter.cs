using System.Diagnostics;

namespace Deface.NET.VideoIO;

internal class VideoWriter
{
    public void CreateVideoFromFrames(List<byte[]> frames, int width, int height, int fps, string outputFilePath)
    {
        var ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-f rawvideo -pix_fmt rgb24 -s {width}x{height} -r {fps} -i - -c:v libvpx -pix_fmt yuv420p -y \"{outputFilePath}\"",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        ffmpegProcess.Start();

        try
        {
            using (var binaryWriter = new BinaryWriter(ffmpegProcess.StandardInput.BaseStream))
            {
                foreach (var frame in frames)
                {
                    binaryWriter.Write(frame);
                }
            }

            ffmpegProcess.StandardInput.Close();
            ffmpegProcess.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        finally
        {
            ffmpegProcess.Close();
        }
    }
}
