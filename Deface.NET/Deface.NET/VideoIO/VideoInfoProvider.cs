using Deface.NET.Configuration.Provider;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Helpers;
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

        return VideoInfoHelper.ConvertOutputToVideoInfo(output, filePath);
    }

    private IExternalProcess GetProcess(string filePath)
    {
        var ffProbePath = _settings.FFProbePath;

        var args = string.Join(" ",
        [
            "-v", "error",
            "-select_streams", "v:0",
            "-show_entries", "stream=width,height,nb_frames,r_frame_rate,avg_frame_rate",
            "-of", "json",
            $"\"{filePath}\""
        ]);

        return _externalProcessFactory.CreateExternalProcess(ffProbePath, args);
    }
}
