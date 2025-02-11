namespace Deface.NET.Configuration.Provider.Interfaces;

internal interface IScopedSettingsProvider
{
    Settings Settings { get; }

    void LoadForCurrentScope(ProcessingType processingType, Action<Settings>? action = null);
}
