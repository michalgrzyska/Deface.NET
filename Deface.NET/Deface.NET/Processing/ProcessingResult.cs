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
    /// Gets the face threshold used for processing.
    /// </summary>
    public float FaceThreshold { get; private set; }

    /// <summary>
    /// Gets the license plate threshold used for processing.
    /// </summary>
    public float LicensePlateThreshold { get; private set; }

    /// <summary>
    /// Gets the frames per second (FPS) during processing.
    /// </summary>
    public double Fps { get; private set; }

    internal ProcessingResult(string inputFile, string outputFile, TimeSpan processingTime, float faceThreshold, float licensePlateThreshold, double fps)
    {
        InputFile = inputFile;
        OutputFile = outputFile;
        ProcessingTime = processingTime;
        LicensePlateThreshold = licensePlateThreshold;
        FaceThreshold = faceThreshold;
        Fps = fps;
    }
}