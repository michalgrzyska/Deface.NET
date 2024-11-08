using Deface.NET.Configuration;
using Deface.NET.Utils;

namespace Deface.NET;

/// <summary>
/// Deface settings.
/// </summary>
public class Settings : IValidable
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
    /// Represents a file format of image if image processing is used.
    /// Does not affect the extension provided in output filename.
    /// For example, if <see cref="ImageFormat.Jpeg(int)"/> is provided,
    /// but output path points to ".png" file, an image will be saved with
    /// ".png" extension but with JPEG compression.
    /// </summary>
    public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

    /// <summary>
    /// Location of FFMpeg executable file.
    /// </summary>
    public string FFMpegPath { get; set; } = string.Empty;

    /// <summary>
    /// Location of FFProbe executable file.
    /// </summary>
    public string FFProbePath { get; set; } = string.Empty;

    internal Settings(Action<Settings>? builderAction, Platform platform)
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

    internal Settings Clone()
    {
        return (Settings)MemberwiseClone();
    }

    /// <inheritdoc/>
    public void Validate()
    {
        ValidationHelper.MustBeGreaterOrEqualTo(Threshold, 0, nameof(Threshold));
        ValidationHelper.MustBeLessThanOrEqualTo(Threshold, 1, nameof(Threshold));

        ValidationHelper.MustBeGreaterOrEqualTo(RunDetectionEachNFrames, 1, nameof(RunDetectionEachNFrames));

        ValidationHelper.MustBeGreaterOrEqualTo(MaskScale, 1, nameof(MaskScale));

        ValidationHelper.ValidateFilePath(FFMpegPath, nameof(FFMpegPath));
        ValidationHelper.ValidateFilePath(FFProbePath, nameof(FFProbePath));

        ImageFormat.Validate();
    }
}