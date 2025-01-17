using Deface.NET.Common;
using Google.Protobuf.WellKnownTypes;

namespace Deface.NET.Configuration.Validation.Validators;

internal class FFProbePathValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings)
    {
        ValidationHelper.MustNotBeNullOrWhiteSpace(settings.FFProbePath, nameof(Settings.FFProbePath));
    }
}
