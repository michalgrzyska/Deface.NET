namespace Deface.NET.PerformanceTestRunner.Config;
public abstract class VideoTestScenarioBase(string videoPath)
{
    public string VideoPath { get; } = videoPath;

    public ProcessingResult Run(Action<Settings>? action = default)
    {
        var defaceService = DefaceProvider.GetDefaceService();
        var output = $"{Guid.NewGuid()}.mp4";
        var result = defaceService.ProcessVideo(VideoPath, "test.mp4");

        File.Delete(output);

        return result;
    }
}
