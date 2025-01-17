
namespace Deface.NET.Configuration.Provider.Interfaces;

internal interface ISettingsProvider
{
    Settings Settings { get; }
    void Initialize(Action<Settings> builder);
}
