using Deface.NET.Common;
using Google.Protobuf.WellKnownTypes;

namespace Deface.NET.Configuration.Validation.Validators;

internal class ThresholdValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings, ProcessingType processingType)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(settings.Threshold, 0, nameof(Settings.Threshold));
        ValidationHelper.MustBeLessThanOrEqualTo(settings.Threshold, 1, nameof(Settings.Threshold));
    }
}
