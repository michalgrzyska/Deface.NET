using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Configuration.Validation;

namespace Deface.NET.Configuration.Provider;

internal class SettingsProvider(ISettingsValidator settingsValidator) : ISettingsProvider
{
    private readonly ISettingsValidator _settingsValidator = settingsValidator;

    private Settings? _settings;

    public Settings Settings
    {
        get => _settings ?? throw new InvalidOperationException("Settings are not initialized.");
        private set => _settings = value;
    }

    public void Initialize(Action<Settings> builder)
    {
        if (_settings is not null) 
        {
            throw new InvalidOperationException("Settings are already initialized.");
        }

        Settings settings = new(builder);
        _settingsValidator.Validate(settings);

        Settings = settings;
    }
}
