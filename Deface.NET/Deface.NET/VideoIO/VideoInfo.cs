using Deface.NET.Utils;
using Deface.NET.VideoIO.Models;
using System.Text.Json;

namespace Deface.NET.VideoIO;

internal record VideoInfo(int Width, int Height, int TotalFrames, float TargetFps, float AverageFps)
{
    public static async Task<VideoInfo> GetInfo(string filePath, Settings settings)
    {
        using ExternalProcess process = GetProcess(filePath, settings);

        var processOutput = await process.ExecuteWithOutput();
        var output = JsonSerializer.Deserialize<VideoInfoOutput>(processOutput)!;

        return ConvertOutputToVideoInfo(output);
    }

    private static ExternalProcess GetProcess(string filePath, Settings settings)
    {
        string ffProbePath = settings.FFMpegConfig.GetCurrentConfig().FFProbePath;

        string args = string.Join(" ",
        [
            "-v", "error",
            "-select_streams", "v:0",
            "-show_entries", "stream=width,height,nb_frames,r_frame_rate,avg_frame_rate",
            "-of", "json",
            $"\"{filePath}\""
        ]);

        return new ExternalProcess(ffProbePath, args);
    }

    private static VideoInfo ConvertOutputToVideoInfo(VideoInfoOutput output)
    {
        var stream = output.Streams.FirstOrDefault() ?? throw new InvalidOperationException("Video has no streams.");

        var width = stream.Width;
        var height = stream.Height;
        var frames = int.Parse(stream.Frames);
        var targetFps = ParseFrameRateString(stream.TargetFrameRate);
        var averageFps = ParseFrameRateString(stream.AverageFrameRate);

        return new(width, height, frames, targetFps, averageFps);
    }

    private static float ParseFrameRateString(string str)
    {
        var numberParts = str.Split('/').Select(int.Parse).ToArray();
        return (float)numberParts[0] / numberParts[1];
    }
}