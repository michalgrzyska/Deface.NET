using Deface.NET.Configuration.Validation.Validators;
using Deface.NET.System.ExternalProcessing;

namespace Deface.NET.Configuration.Validation;

internal class SettingsValidator : ISettingsValidator
{
    private readonly List<ISettingsPropertyValidator> _validators = [];

    public SettingsValidator(IExternalProcessFactory externalProcessFactory)
    {
        RegisterValidator(new ThresholdValidator());
        RegisterValidator(new RunDetectionEachNFramesValidator());
        RegisterValidator(new MaskScaleValidator());
        RegisterValidator(new FFMpegPathValidator(externalProcessFactory));
        RegisterValidator(new FFProbePathValidator(externalProcessFactory));
    }

    public void Validate(Settings settings)
    {
        try
        {
            foreach (var validator in _validators)
            {
                validator.Validate(settings);
            }
        }
        catch (Exception ex)
        {
            throw new DefaceException(ex.Message, ex);
        }
    }

    private void RegisterValidator(ISettingsPropertyValidator validator)
    {
        _validators.Add(validator);
    }
}
