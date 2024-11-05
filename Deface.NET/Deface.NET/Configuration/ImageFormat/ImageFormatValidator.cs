
using Deface.NET.Utils;

namespace Deface.NET.Configuration;

internal static class ImageFormatValidator
{
    public static void Validate(ImageFormat imageFormat)
    {
        ValidateQuality(imageFormat.Quality);
    }

    private static void ValidateQuality(int quality)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(quality, 1, nameof(ImageFormat.Quality));
        ValidationHelper.MustBeLessThanOrEqualTo(quality, 100, nameof(ImageFormat.Quality));
    }
}
