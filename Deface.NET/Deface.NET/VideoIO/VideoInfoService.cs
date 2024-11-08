using Deface.NET.Configuration.Provider;
using Deface.NET.Utils;
using Deface.NET.VideoIO.Models;
using System.Text.Json;

namespace Deface.NET.VideoIO;

internal class VideoInfoService(ScopedSettingsProvider settingsProvider)
{
    private readonly Settings _settings = settingsProvider.Settings;

    public async Task<VideoInfo> GetInfo(string filePath)
    {
        using ExternalProcess process = GetProcess(filePath);

        var processOutput = await process.ExecuteWithOutput();
        var output = JsonSerializer.Deserialize<VideoInfoOutput>(processOutput)!;

        return ConvertOutputToVideoInfo(output);
    }

    private ExternalProcess GetProcess(string filePath)
    {
        string ffProbePath = _settings.FFProbePath;

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
