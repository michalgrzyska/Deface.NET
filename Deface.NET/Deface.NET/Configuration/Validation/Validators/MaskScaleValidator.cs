using Deface.NET.Common;

namespace Deface.NET.Configuration.Validation.Validators;

internal class MaskScaleValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings, ProcessingType processingType)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(settings.MaskScale, 1, nameof(Settings.MaskScale));
    }
}
