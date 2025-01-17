using Deface.NET.Common;
using Google.Protobuf.WellKnownTypes;

namespace Deface.NET.Configuration.Validation.Validators;

internal class FFMpegPathValidator : ISettingsPropertyValidator
{
    public void Validate(Settings settings)
    {
        ValidationHelper.MustNotBeNullOrWhiteSpace(settings.FFMpegPath, nameof(Settings.FFMpegPath));
    }
}
