using Deface.NET.Configuration.Provider;
using Deface.NET.System.ExternalProcessing;
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
        GetVideoEncodersFromOutput(processOutput);
    }

    private IExternalProcess GetProcess()
    {
        var ffProbePath = _settings.FFMpegPath;
        var args = "-encoders";

        return _externalProcessFactory.CreateExternalProcess(ffProbePath, args, redirectStandardInput: true);
    }

    private string[] GetVideoEncodersFromOutput(string output)
    {
        var encodersAllLines = output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();

        var separatorIndex = encodersAllLines.IndexOf(LegendSeparator);

        if (separatorIndex == -1)
        {
            throw new Exception("Separator not found. FFMpeg result may be corruped.");
        }

        var encodersLines = encodersAllLines.Skip(separatorIndex + 1);
        var videoEncodersLines = encodersLines.Where(x => x[0] == 'V');
        var videoEncoders = videoEncodersLines.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).ElementAt(1));

        return [.. videoEncoders];
    }
}
