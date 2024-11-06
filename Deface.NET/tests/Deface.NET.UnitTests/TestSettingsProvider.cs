using Deface.NET.Utils;

namespace Deface.NET.UnitTests;

internal sealed class TestSettingsProvider : IDisposable
{
    private readonly string _testFFMpegFile;
    private readonly string _testFFProbeFile;

    public TestSettingsProvider()
    {
        _testFFMpegFile = Path.GetTempFileName();
        _testFFProbeFile = Path.GetTempFileName();
    }

    public Settings GetSettings(Platform platform = Platform.Windows)
    {
        Action<Settings> action = settings =>
        {
            settings.FFMpegConfig.Windows.FFMpegPath = _testFFMpegFile;
            settings.FFMpegConfig.Windows.FFProbePath = _testFFProbeFile;

            settings.FFMpegConfig.Linux.FFMpegPath = _testFFMpegFile;
            settings.FFMpegConfig.Linux.FFProbePath = _testFFProbeFile;
        };

        return new(action, platform);
    }

    public void Dispose()
    {
        File.Delete(_testFFMpegFile);
        File.Delete(_testFFProbeFile);
    }
}
