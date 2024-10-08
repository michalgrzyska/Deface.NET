namespace Deface.NET;

/// <summary>
/// Deface settings.
/// </summary>
public class Settings
{
    /// <summary>
    /// Specifies amount of information provided via logging mechanism. 
    /// Default: <see cref="DefaceLoggingLevel.Detailed"/>
    /// </summary>
    public DefaceLoggingLevel LoggingLevel { get; set; } = DefaceLoggingLevel.Detailed;

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
    /// Detection threshold. Change that value to adjust detection level (e.g. 0.5 = 50%)
    /// Default: 0.2f
    /// </summary>
    public float Threshold { get; set; } = 0.2f;

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
    /// Use this param to enable automatic frame rescaling for object detection. For example setting RescaleVideoWithShorterSideEqualsTo to 720 means that
    /// a frame of original size 1920x1080 will be rescaled to 1280x720 for the detection process. Output video remains the same size and quality.
    /// If shorter side length is smaller than a value given in RescaleVideoWithShorterSideEqualsTo, frame size remains the same. This process
    /// can affect the quality of detection, but also improves the performance for high-res videos.
    /// </summary>
    public int? RescaleVideoWithShorterSideEqualsTo { get; set; }

    /// <summary>
    /// Scale the size for anonimization area. The bigger the value is, the bigger area around a detected object is redacted to ensure the redaction is proper. 
    /// Default: 1.2f.
    /// </summary>
    public float MaskScale { get; set; } = 1.2f;

    internal Settings(Action<Settings>? builderAction)
    {
        if (builderAction is not null)
        {
            builderAction(this);
            Validate();
        }
    }

    private void Validate()
    {
        if (Threshold < 0 || Threshold > 1)
        {
            throw new InvalidDataException($"{nameof(Threshold)} must be in range from 0 to 1.");
        }

        if (RunDetectionEachNFrames <= 0)
        {
            throw new InvalidDataException($"{nameof(RunDetectionEachNFrames)} must be 1 or greater.");
        }

        if (RescaleVideoWithShorterSideEqualsTo.HasValue && RescaleVideoWithShorterSideEqualsTo <= 0)
        {
            throw new InvalidDataException($"{nameof(RescaleVideoWithShorterSideEqualsTo)} must be 1 or greater.");
        }

        if (MaskScale < 1)
        {
            throw new InvalidDataException($"{nameof(MaskScale)} must at least 1.0");
        }
    }
}