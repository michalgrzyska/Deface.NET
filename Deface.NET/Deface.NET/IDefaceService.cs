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
    /// <returns>Basic information of processed video.</returns>
    public ProcessingResult ProcessVideo(string inputVideoFilePath, string outputVideoFilePath);

    /// <summary>
    /// Processes image for objects detection and anomitization.
    /// </summary>
    /// <param name="inputPhotoFilePath">Location of input file.</param>
    /// <param name="outputPhotoFilePath">Location of output file.</param>
    /// <returns>Basic information of processed image.</returns>
    public ProcessingResult ProcessImage(string inputPhotoFilePath, string outputPhotoFilePath);
}
