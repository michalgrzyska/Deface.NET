using Deface.NET.IntegrationTests.Config;
using Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg;

internal class FFProbeVideoInfo
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TotalFrames { get; private set; }
    public long Size { get; private set; }
    public float FrameRate { get; private set; }

    private FFProbeVideoInfo(int width, int height, int totalFrames, long size, float frameRate)
    {
        Width = width;
        Height = height;
        TotalFrames = totalFrames;
        Size = size;
        FrameRate = frameRate;
    }

    public static async Task<FFProbeVideoInfo> Get(string videoPath)
    {
        using var process = GetProcess(videoPath);
        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        var videoInfo = JsonSerializer.Deserialize<FFProbeOutput>(output);
        var stream = videoInfo.Streams.FirstOrDefault();
        var totalFrames = int.TryParse(stream.NbFrames, out int frames) ? frames : 0;

        return new(stream.Width, stream.Height, totalFrames, videoInfo.Format.Size, ParseFrameRateString(stream.AverageFrameRate));
    }

    private static Process GetProcess(string videoPath)
    {
        var arguments = string.Join(" ",
            "-v error",
            "-select_streams v:0",
            "-show_entries stream=width,height,nb_frames,avg_frame_rate",
            "-show_entries format=size",
            "-print_format json",
            $"\"{videoPath}\""
        );

        return new()
        {
            StartInfo = new()
            {
                FileName = ExecutablePath.FFProbe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
    }

    private static float ParseFrameRateString(string str)
    {
        var numberParts = str.Split('/').Select(int.Parse).ToArray();
        var result = (float)numberParts[0] / numberParts[1];

        return result;
    }
}
