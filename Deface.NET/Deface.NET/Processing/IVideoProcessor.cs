namespace Deface.NET.Processing;

internal interface IVideoProcessor : IDisposable
{
    ProcessingResult Process(string inputPath, string outputPath);
}