using Deface.NET.Processing;
using OpenCvSharp;

namespace Deface.NET;

/// <summary>
/// Represents the result of a Deface processing operation.
/// </summary>
public class ProcessingResult
{
    /// <summary>
    /// Gets the input file path.
    /// </summary>
    public string InputFile { get; private set; }

    /// <summary>
    /// Gets the output file path.
    /// </summary>
    public string OutputFile { get; private set; }

    /// <summary>
    /// Gets the time taken for processing.
    /// </summary>
    public TimeSpan ProcessingTime { get; private set; }

    /// <summary>
    /// Gets the dimensions of the input file.
    /// </summary>
    public Dimensions InputDimensions { get; private set; }

    /// <summary>
    /// Gets the dimensions after processing.
    /// </summary>
    public Dimensions ProcessingDimensions { get; private set; }

    /// <summary>
    /// Gets the threshold used for processing.
    /// </summary>
    public float Threshold { get; private set; }

    /// <summary>
    /// Gets the frames per second (FPS) during processing.
    /// </summary>
    public double Fps { get; private set; }

    internal ProcessingResult(string inputFile, string outputFile, TimeSpan processingTime, Size inputSize, Size processingSize, float threshold, double fps)
    {
        InputFile = inputFile;
        OutputFile = outputFile;
        ProcessingTime = processingTime;
        InputDimensions = inputSize.ToDimensions();
        ProcessingDimensions = processingSize.ToDimensions();
        Threshold = threshold;
        Fps = fps;
    }
}

/// <summary>
/// Represents the dimensions of a media file or a processing frame.
/// </summary>
public class Dimensions
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; private set; }

    internal Dimensions(int width, int height)
    {
        Width = width;
        Height = height;
    }
}