using SkiaSharp;

namespace Deface.NET;

/// <summary>
/// Represents a file format of image if image processing is used.
/// Does not affect the extension provided in output filename.
/// For example, if <see cref="ImageFormat.Jpeg(int)"/> is provided,
/// but output path points to ".png" file, an image will be saved with
/// ".png" extension but with JPEG compression.
/// </summary>
public readonly struct ImageFormat
{
    internal SKEncodedImageFormat Format { get; private init; }
    internal int Quality { get; private init; }

    private ImageFormat(SKEncodedImageFormat format, int quality)
    {
        Format = format;
        Quality = quality;
    }

    /// <summary>
    /// Represents JPEG image type with a given quality.
    /// </summary>
    /// <param name="quality">Image quality. Accepted values: 1 - 100.</param>
    public static ImageFormat Jpeg(int quality = 85)
    {
        if (quality < 1 || quality > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(quality), $"Value must be in range 1-100");
        }

        return new(SKEncodedImageFormat.Jpeg, quality);
    }

    /// <summary>
    /// Represents PNG image type.
    /// </summary>
    public static ImageFormat Png => new(SKEncodedImageFormat.Png, 100);
}
