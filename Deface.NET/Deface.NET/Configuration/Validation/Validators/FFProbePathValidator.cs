using Deface.NET.Common;
using Deface.NET.System.ExternalProcessing;

namespace Deface.NET.Configuration.Validation.Validators;

internal class FFProbePathValidator(IExternalProcessFactory externalProcessFactory) : ISettingsPropertyValidator
{
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public void Validate(Settings settings)
    {
        ValidationHelper.MustNotBeNullOrWhiteSpace(settings.FFProbePath, nameof(Settings.FFProbePath));
        ValidateFFProbeExecutable(settings.FFProbePath);
    }

    private void ValidateFFProbeExecutable(string ffprobePath)
    {
        var process = _externalProcessFactory.CreateExternalProcess(ffprobePath, "-version");
        process.Start();

        var output = process.ExecuteWithOutput();

        if (!output.StartsWith("ffprobe version"))
        {
            throw new ArgumentException($"The provided FFProbe path '{ffprobePath}' is not a valid FFMpeg executable.");
        }
    }
}
