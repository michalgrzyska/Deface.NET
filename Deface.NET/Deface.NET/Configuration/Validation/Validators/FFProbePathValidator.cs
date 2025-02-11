using Deface.NET.Common;
using Deface.NET.System.ExternalProcessing;

namespace Deface.NET.Configuration.Validation.Validators;

internal class FFProbePathValidator(IExternalProcessFactory externalProcessFactory) : ISettingsPropertyValidator
{
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public void Validate(Settings settings, ProcessingType processingType)
    {
        if (processingType != ProcessingType.Video)
        {
            return;
        }

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
            throw new Exception("Returned test string from FFProbe is invalid. Check your FFProbe with -version parameter to make sure it works.");
        }
    }
}
