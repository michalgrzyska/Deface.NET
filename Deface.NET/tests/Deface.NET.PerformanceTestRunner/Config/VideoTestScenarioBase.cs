using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Deface.NET.PerformanceTestRunner.Config;
public abstract class VideoTestScenarioBase(string videoPath)
{
    public string VideoPath { get; } = videoPath;

    public ProcessingResult Run(Action<Settings>? action = default)
    {
        ServiceCollection services = new();

        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.SetMinimumLevel(LogLevel.None);
        });

        services.AddDeface(action);

        var serviceProvider = services.BuildServiceProvider();
        var defaceService = serviceProvider.GetRequiredService<IDefaceService>();
        var output = $"{Guid.NewGuid().ToString()}.mp4";
        var result = defaceService.ProcessVideo(VideoPath, "test.mp4");

        File.Delete(output);

        return result;
    }
}
