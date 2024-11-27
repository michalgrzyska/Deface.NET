namespace Deface.NET.Processing;

internal interface IVideoProcessor
{
    ProcessingResult Process(string inputPath, string outputPath);
}