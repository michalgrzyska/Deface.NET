namespace Deface.NET;

/// <summary>
/// Deface settings.
/// </summary>
public class Settings
{
    /// <summary>
    /// Specifies amount of information provided via logging mechanism. 
    /// Default: <see cref="LoggingLevel.Detailed"/>
    /// </summary>
    public LoggingLevel LoggingLevel { get; set; } = LoggingLevel.Detailed;

    /// <summary>
    /// Specifies the shape of used anonimization.
    /// Default: <see cref="AnonimizationShape.Ellipse"/>
    /// </summary>
    public AnonimizationShape AnonimizationShape { get; set; } = AnonimizationShape.Ellipse;

    /// <summary>
    /// Specifies what type of anonimization will be used on detected objects.
    /// Default: <see cref="AnonimizationMethod.GaussianBlur" />
    /// </summary>
    public AnonimizationMethod AnonimizationMethod { get; set; } = AnonimizationMethod.GaussianBlur;

    /// <summary>
    /// Face detection threshold. Change that value to adjust detection level (e.g. 0.5 = 50%)
    /// Default: 0.2f
    /// </summary>
    public float FaceThreshold { get; set; } = 0.2f;

    /// <summary>
    /// Face detection threshold. Change that value to adjust detection level (e.g. 0.5 = 50%)
    /// Default: 0.2f
    /// </summary>
    public float LicensePlateThreshold { get; set; } = 0.2f;

    /// <summary>
    /// <para>
    ///     Runs object detection only for N-th frame. 
    ///     Setting this param to 2 means that each second frame will be processed, 3 = each third etc.
    ///     Applies for video only.
    /// </para>
    /// <para>Default: 1 (all frames)</para>
    /// </summary>
    public int RunDetectionEachNFrames { get; set; } = 1;

    /// <summary>
    /// Scale the size for anonimization area. The bigger the value is, the bigger area around a detected object is redacted to ensure the redaction is proper. 
    /// Default: 1.2f.
    /// </summary>
    public float MaskScale { get; set; } = 1.2f;

    /// <summary>
    /// Represents a file format of image if image processing is used.
    /// Does not affect the extension provided in output filename.
    /// For example, if <see cref="ImageFormat.Jpeg(int)"/> is provided,
    /// but output path points to ".png" file, an image will be saved with
    /// ".png" extension but with JPEG compression.
    /// </summary>
    public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

    /// <summary>
    /// Location of FFMpeg executable file. At default it points to "ffmpeg" as a global command.
    /// </summary>
    public string FFMpegPath { get; set; } = "ffmpeg";

    /// <summary>
    /// Location of FFProbe executable file. At default it points to "ffprobe" as a global command.
    /// </summary>
    public string FFProbePath { get; set; } = "ffprobe";

    /// <summary>
    /// Hardware used for the object detection. NOTE: Cannot be overriden locally, Deface.NET always uses global setting.
    /// </summary>
    public Hardware Hardware { get; set; } = Hardware.Cpu();

    /// <summary>
    /// Codec used for encoding a processed video.
    /// </summary>
    public EncodingCodec EncodingCodec { get; set; } = EncodingCodec.VP9;

    /// <summary>
    /// Disables logging for commercial features.
    /// </summary>
    public bool HideCommercialFeaturesInfo { get; set; } = false;

    /// <summary>
    /// For some cases (e.g. Azure Functions) Deface.NET may not work with default settings for working directory.
    /// In that case a custom working directory may be provided.
    /// <para>NOTE: Cannot be overriden locally, Deface.NET always uses global setting.</para>
    /// </summary>
    public string? CustomBaseDirectory { get; set; }

    internal Settings(Action<Settings>? builderAction)
    {
        ApplyAction(builderAction);
    }

    internal void ApplyAction(Action<Settings>? builderAction)
    {
        if (builderAction is not null)
        {
            builderAction(this);
        }
    }

    internal Settings Clone()
    {
        return (Settings)MemberwiseClone();
    }
}