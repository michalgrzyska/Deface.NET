using Deface.NET.Configuration.FFMpeg;
using Deface.NET.Utils;

namespace Deface.NET.Configuration;

internal static class SettingsValidator
{
    public static void Validate(Settings settings)
    {
        ValidateThreshold(settings.Threshold);
        ValidateRunDetectionEachNFrames(settings.RunDetectionEachNFrames);
        ValidateMaskScale(settings.MaskScale);

        FFMpegConfigValidator.Validate(settings.FFMpegConfig, nameof(Settings.FFMpegConfig));
    }

    private static void ValidateThreshold(float threshold)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(threshold, 0, nameof(Settings.Threshold));
        ValidationHelper.MustBeLessThan(threshold, 1, nameof(Settings.Threshold));
    }

    private static void ValidateRunDetectionEachNFrames(int runDetectionEachNFrames)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(runDetectionEachNFrames, 1, nameof(Settings.RunDetectionEachNFrames));
    }

    private static void ValidateMaskScale(float maskScale)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(maskScale, 1, nameof(Settings.MaskScale));
    }
}
