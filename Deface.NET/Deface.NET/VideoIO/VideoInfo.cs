using Deface.NET.VideoIO.Models;
using Newtonsoft.Json;

namespace Deface.NET.VideoIO;

internal record VideoInfo(int Width, int Height, int TotalFrames, float TargetFps, float AverageFps)
{
    public static async Task<VideoInfo> GetInfo(string filePath)
    {
        using ExternalProcess process = new(
            "ffprobe.exe",
            $"-v error -select_streams v:0 -show_entries stream=width,height,nb_frames,r_frame_rate,avg_frame_rate -of json \"{filePath}\""
        );

        var processOutput = await process.ExecuteWithOutput();
        var output = JsonConvert.DeserializeObject<VideoInfoOutput>(processOutput)!;

        return ConvertOutputToVideoInfo(output);
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