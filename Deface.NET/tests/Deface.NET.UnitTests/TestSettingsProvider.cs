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
            settings.FFMpegPath = _testFFMpegFile;
            settings.FFProbePath = _testFFProbeFile;
        };

        return new(action, platform);
    }

    public void Dispose()
    {
        File.Delete(_testFFMpegFile);
        File.Delete(_testFFProbeFile);
    }
}
