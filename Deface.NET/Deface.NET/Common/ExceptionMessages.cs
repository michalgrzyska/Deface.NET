namespace Deface.NET.Common;

internal static class ExceptionMessages
{
    public const string AppFileNotFound = $"Could not find the required file: {{0}}. This error may occur because the parent application uses a different base directory than the actual application's folder. Refer to https://github.com/michalgrzyska/Deface.NET?tab=readme-ov-file#3-custombasedirectory to learn how to use the {nameof(Settings)}.{nameof(Settings.CustomBaseDirectory)} property to resolve this issue.";

    public const string MustBeGreaterOrEqualTo = "Value must be greater or equal to {0}";
    public const string MustBeLessThanOrEqualTo = "Value must be less than or equal to {0}";
    public const string MustNotBeNullOrWhiteSpace = "{0} must not be null or whitespace/empty";
    public const string FileMustExist = "{0} does not contain a name of existing file";
    public const string DirectoryMustExist = "{0} does not contain a name of existing directory";

    public const string SettingsNotInitalized = "Settings are not initialized.";
    public const string SettingsAlreadyInitalized = "Settings are already initialized.";

    public const string InvalidFFMpegTestOutput = "Returned test string from FFMpeg is invalid. Check your FFMpeg with -version parameter to make sure it works.";
    public const string InvalidFFProbeTestOutput = "Returned test string from FFProbe is invalid. Check your FFProbe with -version parameter to make sure it works.";

    public const string FailedToDecodeImage = "Failed to decode image from file '{0}'.";
    public const string VideoHasNoStreams = "Video has no streams.";

    public const string SeparatorNotFound = "Separator not found. FFMpeg result may be corruped.";
    public const string EncoderNotFound = "Encoder {0} is not available in your FFMpeg build.";

    public const string ErrorWhileWritingFrames = "An error occured while writing frames to a target destination. Ensure your target file destination is valid and accessible by FFMpeg process.";
}
