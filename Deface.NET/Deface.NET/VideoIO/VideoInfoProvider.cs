using Deface.NET.Configuration.Provider;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;
using System.Text.Json;

namespace Deface.NET.VideoIO;

internal class VideoInfoProvider(IScopedSettingsProvider settingsProvider, IExternalProcessFactory externalProcessFactory) : IVideoInfoProvider
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public VideoInfo GetInfo(string filePath)
    {
        using var process = GetProcess(filePath);

        var processOutput = process.ExecuteWithOutput();
        var output = JsonSerializer.Deserialize<VideoInfoOutput>(processOutput)!;

        return ConvertOutputToVideoInfo(output, filePath);
    }

    private IExternalProcess GetProcess(string filePath)
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

        return _externalProcessFactory.CreateExternalProcess(ffProbePath, args);
    }

    private static VideoInfo ConvertOutputToVideoInfo(VideoInfoOutput output, string filePath)
    {
        var stream = output.Streams.FirstOrDefault() ?? throw new InvalidOperationException("Video has no streams.");

        var width = stream.Width;
        var height = stream.Height;
        var frames = int.Parse(stream.Frames);
        var targetFps = ParseFrameRateString(stream.TargetFrameRate);
        var averageFps = ParseFrameRateString(stream.AverageFrameRate);

        return new(width, height, frames, targetFps, averageFps, filePath);
    }

    private static float ParseFrameRateString(string str)
    {
        var numberParts = str.Split('/').Select(int.Parse).ToArray();
        return (float)numberParts[0] / numberParts[1];
    }
}
