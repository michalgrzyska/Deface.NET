using Deface.NET.Common;
using Deface.NET.System.ExternalProcessing;

namespace Deface.NET.Configuration.Validation.Validators;

internal class FFMpegPathValidator(IExternalProcessFactory externalProcessFactory) : ISettingsPropertyValidator
{
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public void Validate(Settings settings, ProcessingType processingType)
    {
        if (processingType != ProcessingType.Video)
        {
            return;
        }

        ValidationHelper.MustNotBeNullOrWhiteSpace(settings.FFMpegPath, nameof(Settings.FFMpegPath));
        ValidateFFMpegExecutable(settings.FFMpegPath);
    }

    private void ValidateFFMpegExecutable(string ffmpegPath)
    {
        var process = _externalProcessFactory.CreateExternalProcess(ffmpegPath, "-version");
        process.Start();

        var output = process.ExecuteWithOutput();

        if (!output.StartsWith("ffmpeg version"))
        {
            throw new Exception(ExceptionMessages.InvalidFFMpegTestOutput);
        }
    }
}
