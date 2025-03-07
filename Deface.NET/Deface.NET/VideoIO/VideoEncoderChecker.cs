using Deface.NET.Common;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Helpers;
using Deface.NET.VideoIO.Interfaces;

namespace Deface.NET.VideoIO;

internal class VideoEncoderChecker(IScopedSettingsProvider settingsProvider, IExternalProcessFactory externalProcessFactory) : IVideoEncoderChecker
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    // FFMpeg result string contains a multiline legend that ends with that string
    private const string LegendSeparator = "------";

    public void CheckFfmpegCodecs()
    {
        using var process = GetProcess();
        var processOutput = process.ExecuteWithOutput();
        var encoders = GetVideoEncodersFromOutput(processOutput);
        var currentCodec = CodecHelper.GetCodecName(_settings.EncodingCodec);

        if (!encoders.Contains(currentCodec))
        {
            throw new InvalidOperationException(string.Format(ExceptionMessages.EncoderNotFound, _settings.EncodingCodec));
        }
    }

    private IExternalProcess GetProcess()
    {
        var ffProbePath = _settings.FFMpegPath;
        var args = "-encoders";

        return _externalProcessFactory.CreateExternalProcess(ffProbePath, args, redirectStandardInput: true);
    }

    private static string[] GetVideoEncodersFromOutput(string output)
    {
        var encodersAllLines = output
            .Split([ "\n", "\r" ], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();

        var separatorIndex = encodersAllLines.IndexOf(LegendSeparator);

        if (separatorIndex == -1)
        {
            throw new InvalidOperationException(ExceptionMessages.SeparatorNotFound);
        }

        var encodersLines = encodersAllLines.Skip(separatorIndex + 1);
        var videoEncodersLines = encodersLines.Where(x => x[0] == 'V');
        var videoEncoders = videoEncodersLines.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).ElementAt(1));

        return [.. videoEncoders];
    }
}
