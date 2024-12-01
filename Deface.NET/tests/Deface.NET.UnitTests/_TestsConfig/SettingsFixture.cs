using Deface.NET.Configuration.Provider;
using NSubstitute;

namespace Deface.NET.UnitTests._TestsConfig;

public sealed class SettingsFixture : IDisposable
{
    private readonly string _ffMpegPath;
    private readonly string _ffProbePath;
    private readonly Action<Settings> _action;

    public Settings Settings => new(_action);
    public Action<Settings> Action => (Action<Settings>)_action.Clone();

    public SettingsFixture()
    {
        _ffMpegPath = Path.GetTempFileName();
        _ffProbePath = Path.GetTempFileName();

        _action = settings =>
        {
            settings.FFMpegPath = Path.GetTempFileName();
            settings.FFProbePath = Path.GetTempFileName();
        };
    }

    public Settings WithAction(Action<Settings>? action = null)
    {
        var settings = Settings;

        if (action is not null)
        {
            settings.ApplyAction(action);
        }

        return settings;
    }

    internal IScopedSettingsProvider GetScopedSettingsProvider(Action<Settings>? action = null)
    {
        var settings = WithAction(action);
        var provider = Substitute.For<IScopedSettingsProvider>();

        provider.Settings.Returns(settings);

        return provider;
    }

    public void Dispose()
    {
        File.Delete(_ffMpegPath);
        File.Delete(_ffProbePath);
    }
}

[CollectionDefinition(nameof(SettingsCollection))]
public class SettingsCollection : ICollectionFixture<SettingsFixture>;