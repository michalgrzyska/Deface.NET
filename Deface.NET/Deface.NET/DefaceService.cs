using Deface.NET.Configuration.Provider;
using Deface.NET.Processing;
using Deface.NET.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET;

internal sealed class DefaceService(IServiceScopeFactory scopeFactory) : IDefaceService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<ProcessingResult> ProcessVideo(string inputPath, string outputPath, Action<Settings>? customSettings = default)
    {
        ValidationHelper.ValidateFilePath(inputPath, nameof(inputPath));
        ValidationHelper.MustNotBeNullOrWhiteSpace(outputPath, nameof(outputPath));

        using var scope = _scopeFactory.CreateUserScope(customSettings);
        using var processor = scope.ServiceProvider.GetRequiredService<VideoProcessor>();

        return await processor.Process(inputPath, outputPath);
    }

    public ProcessingResult ProcessImage(string inputPath, string outputPath, Action<Settings>? customSettings = default)
    {
        ValidationHelper.ValidateFilePath(inputPath, nameof(inputPath));
        ValidationHelper.MustNotBeNullOrWhiteSpace(outputPath, nameof(outputPath));

        using var scope = _scopeFactory.CreateUserScope(customSettings);
        using var processor = scope.ServiceProvider.GetRequiredService<ImageProcessor>();

        return processor.Process(inputPath, outputPath);
    }

    public IEnumerable<ProcessingResult> ProcessImages(string inputDirectory, string outputDirectory, Action<Settings>? customSettings = null)
    {
        ValidationHelper.ValidateDirectoryPath(inputDirectory, nameof(inputDirectory));
        ValidationHelper.ValidateDirectoryPath(outputDirectory, nameof(outputDirectory));

        using var scope = _scopeFactory.CreateUserScope(customSettings);
        using var processor = scope.ServiceProvider.GetRequiredService<ImageProcessor>();

        return processor.ProcessMany(inputDirectory, outputDirectory);
    }
}
