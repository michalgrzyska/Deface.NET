using Deface.NET.Common;
using Google.Protobuf.WellKnownTypes;

namespace Deface.NET.Configuration.Validation.Validators;

internal class RunDetectionEachNFramesValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings)
    {
        ValidationHelper.MustBeGreaterOrEqualTo(settings.RunDetectionEachNFrames, 1, nameof(Settings.RunDetectionEachNFrames));
    }
}
