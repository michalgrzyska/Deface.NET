using Deface.NET.Common;

namespace Deface.NET.Configuration.Validation.Validators;

internal class FaceThresholdValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings, ProcessingType processingType)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(settings.FaceThreshold, 0, nameof(Settings.FaceThreshold));
        ValidationHelper.MustBeLessThanOrEqualTo(settings.FaceThreshold, 1, nameof(Settings.FaceThreshold));
    }
}
