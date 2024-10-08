using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Configuration.Validation;

namespace Deface.NET.Configuration.Provider;

internal class ScopedSettingsProvider(ISettingsProvider settingsProvider, ISettingsValidator settingsValidator) : IScopedSettingsProvider
{
    private readonly ISettingsValidator settingsValidator = settingsValidator;
    private readonly Settings _settings = settingsProvider.Settings;

    private Settings? _scopedSettings;
    private bool _isInitiated;

    public Settings Settings => _scopedSettings ?? _settings;

    public void LoadForCurrentScope(ProcessingType processingType, Action<Settings>? action = default)
    {
        if (_isInitiated)
        {
            return;
        }

        var settingsClone = _settings.Clone();

        if (action != null) 
        {
            settingsClone.ApplyAction(action);
            settingsValidator.Validate(settingsClone, processingType);
        }

        _scopedSettings = settingsClone;
        _isInitiated = true;
    }
}
