using Deface.NET.Common;

namespace Deface.NET.Configuration.Validation.Validators;

internal class RunDetectionEachNFramesValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings, ProcessingType processingType)
    {
        if (processingType != ProcessingType.Video)
        {
            return;
        }

        ValidationHelper.MustBeGreaterOrEqualTo(settings.RunDetectionEachNFrames, 1, nameof(Settings.RunDetectionEachNFrames));
    }
}
