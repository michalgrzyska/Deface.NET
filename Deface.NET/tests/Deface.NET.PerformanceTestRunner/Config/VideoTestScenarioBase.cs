namespace Deface.NET.PerformanceTestRunner.Config;
public abstract class VideoTestScenarioBase(string videoPath)
{
    private string _ffmpegPath = string.Empty;
    private string _ffprobePath = string.Empty;

    public void SetFilesPaths(string ffmpegPath, string ffprobePath)
    {
        _ffmpegPath = ffmpegPath;
        _ffprobePath = ffprobePath;
    }

    public string VideoPath { get; } = videoPath;

    public ProcessingResult Run(Action<Settings>? action = default)
    {
        var actionWithFileSettings = SetFilesPathsInAction(action);
        var defaceService = DefaceProvider.GetDefaceService(actionWithFileSettings);
        var output = $"{Guid.NewGuid()}.mp4";
        var result = defaceService.ProcessVideo(VideoPath, "test.mp4");

        File.Delete(output);

        return result;
    }

    private Action<Settings> SetFilesPathsInAction(Action<Settings>? action = default)
    {
        action ??= (settings) => { };

        action += (Settings settings) =>
        {
            settings.FFMpegPath = _ffmpegPath;
            settings.FFProbePath = _ffprobePath;
        };

        return action;
    }
}
