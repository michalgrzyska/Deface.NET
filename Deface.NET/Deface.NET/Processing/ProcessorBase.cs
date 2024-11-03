namespace Deface.NET.Processing;

internal abstract class ProcessorBase(Settings settings)
{
    private readonly Settings _settings = settings;
    private Settings? _overridenSettings;

    protected Settings Settings => _overridenSettings ?? _settings;

    protected void ApplyScopedSettings(Action<Settings>? action)
    {
        if (action is null)
        {
            return;
        }

        var settingsClone = _settings.ShallowCopy();
        settingsClone.ApplyAction(action);

        _overridenSettings = settingsClone;
    }

}
