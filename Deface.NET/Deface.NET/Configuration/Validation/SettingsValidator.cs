using Deface.NET.Configuration.Validation.Validators;

namespace Deface.NET.Configuration.Validation;

internal class SettingsValidator : ISettingsValidator
{
    private readonly List<ISettingsPropertyValidator> _validators = [];

    public SettingsValidator()
    {
        RegisterValidator(new ThresholdValidator());
        RegisterValidator(new RunDetectionEachNFramesValidator());
        RegisterValidator(new MaskScaleValidator());
        RegisterValidator(new FFMpegPathValidator());
        RegisterValidator(new FFProbePathValidator());
    }

    public void Validate(Settings settings)
    {
        foreach (var validator in _validators)
        {
            validator.Validate(settings);
        }
    }

    private void RegisterValidator(ISettingsPropertyValidator validator)
    {
        _validators.Add(validator);
    }
}
