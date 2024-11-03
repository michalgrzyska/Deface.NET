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
    /// Gets the threshold used for processing.
    /// </summary>
    public float Threshold { get; private set; }

    /// <summary>
    /// Gets the frames per second (FPS) during processing.
    /// </summary>
    public double Fps { get; private set; }

    internal ProcessingResult(string inputFile, string outputFile, TimeSpan processingTime, float threshold, double fps)
    {
        InputFile = inputFile;
        OutputFile = outputFile;
        ProcessingTime = processingTime;
        Threshold = threshold;
        Fps = fps;
    }
}