using Deface.NET.Common;
using Deface.NET.Configuration.Provider.Interfaces;

namespace Deface.NET.Configuration.Provider;

internal class SettingsProvider : ISettingsProvider
{
    private Settings? _settings;

    public Settings Settings
    {
        get => _settings ?? throw new InvalidOperationException(ExceptionMessages.SettingsNotInitalized);
        private set => _settings = value;
    }

    public void Initialize(Action<Settings>? builder)
    {
        if (_settings is not null) 
        {
            throw new InvalidOperationException(ExceptionMessages.SettingsAlreadyInitalized);
        }

        Settings = new(builder);
    }
}
