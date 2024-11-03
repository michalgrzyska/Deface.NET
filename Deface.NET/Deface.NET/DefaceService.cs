using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET;

internal sealed class DefaceService(IServiceProvider serviceProvider) : IDefaceService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<ProcessingResult> ProcessVideo(string inputPath, string outputPath, Action<Settings>? customSettings = default)
    {
        ValidateFileInput(inputPath, outputPath);

        using var processor = _serviceProvider.GetRequiredService<VideoProcessor>();
        return await processor.Process(inputPath, outputPath, customSettings);
    }

    public ProcessingResult ProcessImage(string inputPath, string outputPath, Action<Settings>? customSettings = default)
    {
        ValidateFileInput(inputPath, outputPath);

        using var processor = _serviceProvider.GetRequiredService<ImageProcessor>();
        return processor.Process(inputPath, outputPath, customSettings);
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
