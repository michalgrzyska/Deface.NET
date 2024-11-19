
namespace Deface.NET.Configuration.Provider;

internal interface IScopedSettingsProvider
{
    Settings Settings { get; }

    void Init(Action<Settings>? action = null);
}
