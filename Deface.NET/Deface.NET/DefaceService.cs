using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;
using OpenCvSharp;

namespace Deface.NET;

internal sealed class DefaceService : IDefaceService
{
    private readonly IServiceProvider _serviceProvider;

    public DefaceService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ProcessingResult> ProcessVideo(string inputVideoFilePath, string outputVideoFilePath, Action<Settings>? customSettings = default)
    {
        ValidateFileInput(inputVideoFilePath, outputVideoFilePath);

        using var processor = _serviceProvider.GetRequiredService<VideoProcessor>();
        return await processor.Process(inputVideoFilePath, outputVideoFilePath, customSettings);
    }

    public ProcessingResult ProcessImage(string inputPhotoFilePath, string outputPhotoFilePath, Action<Settings>? customSettings = default)
    {
        ValidateFileInput(inputPhotoFilePath, outputPhotoFilePath);

        using var processor = _serviceProvider.GetRequiredService<ImageProcessor>();
        return processor.Process(inputPhotoFilePath, outputPhotoFilePath, customSettings);
    }

    public IEnumerable<ProcessingResult> ProcessImages(string inputDirectory, string outputDirectory, Action<Settings>? customSettings = null)
    {
        ValidateDirectoryInput(inputDirectory, outputDirectory);

        using var processor = _serviceProvider.GetRequiredService<ImageProcessor>();
        return processor.ProcessMany(inputDirectory, outputDirectory, customSettings);
    }

    private static void ValidateFileInput(string inputFilePath, string outputFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inputFilePath, nameof(inputFilePath));
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFilePath, nameof(outputFilePath));

        if (!File.Exists(inputFilePath))
        {
            throw new FileNotFoundException($"File {inputFilePath} does not exist.");
        }
    }

    private static void ValidateDirectoryInput(string inputDirectory, string outputDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inputDirectory, nameof(inputDirectory));
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory, nameof(outputDirectory));

        if (!Directory.Exists(inputDirectory))
        {
            throw new DirectoryNotFoundException($"Directory {inputDirectory} does not exist.");
        }

        if (!Directory.Exists(outputDirectory)) 
        {
            Directory.CreateDirectory(outputDirectory);
        }
    }
}
