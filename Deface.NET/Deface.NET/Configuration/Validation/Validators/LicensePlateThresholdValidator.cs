using Deface.NET.Common;

namespace Deface.NET.Configuration.Validation.Validators;

internal class LicensePlateThresholdValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings, ProcessingType processingType)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(settings.LicensePlateThreshold, 0, nameof(Settings.LicensePlateThreshold));
        ValidationHelper.MustBeLessThanOrEqualTo(settings.LicensePlateThreshold, 1, nameof(Settings.LicensePlateThreshold));
    }
}
