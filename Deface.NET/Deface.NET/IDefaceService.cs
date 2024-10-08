namespace Deface.NET;

/// <summary>
/// A service for media processing.
/// </summary>
public interface IDefaceService
{
    /// <summary>
    /// Processes video for objects detection and anomitization.
    /// </summary>
    /// <param name="inputVideoFilePath">Location of input file.</param>
    /// <param name="outputVideoFilePath">Location of output file.</param>
    /// <param name="customSettings">Override global Deface settings.</param>
    /// <returns>Basic information of processed video.</returns>
    public ProcessingResult ProcessVideo(string inputVideoFilePath, string outputVideoFilePath, Action<Settings>? customSettings = default);

    /// <summary>
    /// Processes image for objects detection and anomitization.
    /// </summary>
    /// <param name="inputImageFilePath">Location of input file.</param>
    /// <param name="outputImageFilePath">Location of output file.</param>
    /// <param name="customSettings">Override global Deface settings.</param>
    /// <returns>Basic information of processed image.</returns>
    public ProcessingResult ProcessImage(string inputImageFilePath, string outputImageFilePath, Action<Settings>? customSettings = default);

    /// <summary>
    /// Processes all images found in an input directory and saves them to output directory.
    /// </summary>
    /// <param name="inputDirectory">Directory containing input images.</param>
    /// <param name="outputDirectory">Directory for output images.</param>
    /// <param name="customSettings">Override global Deface settings.</param>
    /// <returns>Collection of basic information of processed images.</returns>
    public IEnumerable<ProcessingResult> ProcessImages(string inputDirectory, string outputDirectory, Action<Settings>? customSettings = default);
}
