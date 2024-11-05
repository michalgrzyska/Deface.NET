namespace Deface.NET.UnitTests.SettingsTests;

public sealed class TestSettingsProvider : IDisposable
{
    private readonly string _testFFMpegFile;
    private readonly string _testFFProbeFile;

    public TestSettingsProvider()
    {
        _testFFMpegFile = Path.GetTempFileName();
        _testFFProbeFile = Path.GetTempFileName();
    }

    public Settings GetSettings()
    {
        Action<Settings> action = settings =>
        {
            settings.FFMpegConfig.Windows.FFMpegPath = _testFFMpegFile;
            settings.FFMpegConfig.Windows.FFProbePath = _testFFProbeFile;
        };

        return new(action);
    }

    public void Dispose()
    {
        File.Delete(_testFFMpegFile);
        File.Delete(_testFFProbeFile);
    }
}
