namespace Deface.NET.Processing;

internal interface IImageProcessor
{
    ProcessingResult Process(string inputPath, string outputPath);
    List<ProcessingResult> ProcessMany(string inputDirectory, string outputDirectory);
}