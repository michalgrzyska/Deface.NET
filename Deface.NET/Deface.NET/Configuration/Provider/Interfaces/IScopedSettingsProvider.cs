namespace Deface.NET.Configuration.Provider.Interfaces;

internal interface IScopedSettingsProvider
{
    Settings Settings { get; }

    void Init(Action<Settings>? action = null);
}
