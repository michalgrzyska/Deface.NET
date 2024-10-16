using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET;

internal sealed class DefaceService(IServiceProvider serviceProvider) : IDefaceService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ProcessingResult ProcessVideo(string inputVideoFilePath, string outputVideoFilePath, Action<Settings>? customSettings = default)
    {
        ValidateInput(inputVideoFilePath, outputVideoFilePath);

        using var processor = _serviceProvider.GetRequiredService<VideoProcessor>();
        return processor.Process(inputVideoFilePath, outputVideoFilePath, customSettings);
    }

    public ProcessingResult ProcessImage(string inputPhotoFilePath, string outputPhotoFilePath, Action<Settings>? customSettings = default)
    {
        ValidateInput(inputPhotoFilePath, outputPhotoFilePath);

        using var processor = _serviceProvider.GetRequiredService<ImageProcessor>();
        return processor.Process(inputPhotoFilePath, outputPhotoFilePath, customSettings);
    }

    private static void ValidateInput(string inputFilePath, string outputFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inputFilePath, nameof(inputFilePath));
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFilePath, nameof(outputFilePath));

        if (!File.Exists(inputFilePath))
        {
            throw new FileNotFoundException($"File {inputFilePath} does not exist.");
        }
    }
}
