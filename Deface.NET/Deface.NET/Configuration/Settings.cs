using Deface.NET.Configuration.FFMpeg;

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
    /// Scale the size for anonimization area. The bigger the value is, the bigger area around a detected object is redacted to ensure the redaction is proper. 
    /// Default: 1.2f.
    /// </summary>
    public float MaskScale { get; set; } = 1.2f;

    /// <summary>
    /// Configuration settings for FFMpeg and FFProbe for all platforms. Multiple platforms can be configured, if the target platform is not known.
    /// </summary>
    public FFMpegConfig FFMpegConfig { get; set; } = new();

    internal Settings(Action<Settings>? builderAction)
    {
        ApplyAction(builderAction);
    }

    internal void ApplyAction(Action<Settings>? builderAction)
    {
        if (builderAction is not null)
        {
            builderAction(this);
            Validate();
        }
    }

    internal Settings ShallowCopy()
    {
        return (Settings)MemberwiseClone();
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

        if (MaskScale < 1)
        {
            throw new InvalidDataException($"{nameof(MaskScale)} must at least 1.0.");
        }

        FFMpegConfigValidator.Validate(FFMpegConfig, nameof(FFMpegConfig));
    }
}