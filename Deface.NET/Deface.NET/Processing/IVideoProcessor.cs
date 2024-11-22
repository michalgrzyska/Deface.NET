namespace Deface.NET.Processing;

internal interface IVideoProcessor : IDisposable
{
    Task<ProcessingResult> Process(string inputPath, string outputPath);
}