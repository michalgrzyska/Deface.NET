namespace Deface.NET.Configuration.Provider;

internal class ScopedSettingsProvider(Settings settings) : IScopedSettingsProvider
{
    private readonly Settings _settings = settings;

    private Settings? _scopedSettings;
    private bool _isInitiated;

    public Settings Settings => _scopedSettings ?? _settings;

    public void Init(Action<Settings>? action = default)
    {
        if (_isInitiated || action == default)
        {
            return;
        }

        var settingsClone = _settings.Clone();
        settingsClone.ApplyAction(action);

        _scopedSettings = settingsClone;
        _isInitiated = true;
    }
}
