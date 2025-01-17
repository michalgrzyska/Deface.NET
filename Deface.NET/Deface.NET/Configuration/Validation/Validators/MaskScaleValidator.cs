using Deface.NET.Common;
using Google.Protobuf.WellKnownTypes;

namespace Deface.NET.Configuration.Validation.Validators;

internal class MaskScaleValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(settings.MaskScale, 1, nameof(Settings.MaskScale));
    }
}
