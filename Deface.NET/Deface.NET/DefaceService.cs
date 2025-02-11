using Deface.NET.Common;
using Deface.NET.Configuration;
using Deface.NET.Configuration.Provider;
using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET;

internal sealed class DefaceService(IServiceScopeFactory scopeFactory) : IDefaceService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public ProcessingResult ProcessVideo(string inputPath, string outputPath, Action<Settings>? customSettings = default)
    {
        try
        {
            ValidationHelper.ValidateFilePath(inputPath, nameof(inputPath));
            ValidationHelper.MustNotBeNullOrWhiteSpace(outputPath, nameof(outputPath));

            using var scope = _scopeFactory.CreateUserScope(ProcessingType.Video, customSettings);
            var processor = scope.ServiceProvider.GetRequiredService<IVideoProcessor>();

            return processor.Process(inputPath, outputPath);
        }
        catch (Exception ex)
        {
            throw new DefaceException(ex);
        }
    }

    public ProcessingResult ProcessImage(string inputPath, string outputPath, Action<Settings>? customSettings = default)
    {
        try
        {
            ValidationHelper.ValidateFilePath(inputPath, nameof(inputPath));
            ValidationHelper.MustNotBeNullOrWhiteSpace(outputPath, nameof(outputPath));

            using var scope = _scopeFactory.CreateUserScope(ProcessingType.Image, customSettings);
            var processor = scope.ServiceProvider.GetRequiredService<IImageProcessor>();

            return processor.Process(inputPath, outputPath);
        }
        catch (Exception ex)
        {
            throw new DefaceException(ex);
        }
    }

    public IEnumerable<ProcessingResult> ProcessImages(string inputDirectory, string outputDirectory, Action<Settings>? customSettings = null)
    {
        try
        {
            ValidationHelper.ValidateDirectoryPath(inputDirectory, nameof(inputDirectory));
            ValidationHelper.ValidateDirectoryPath(outputDirectory, nameof(outputDirectory));

            using var scope = _scopeFactory.CreateUserScope(ProcessingType.Image, customSettings);
            var processor = scope.ServiceProvider.GetRequiredService<IImageProcessor>();

            return processor.ProcessMany(inputDirectory, outputDirectory);
        }
        catch (Exception ex)
        {
            throw new DefaceException(ex);
        }
    }
}
