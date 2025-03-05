using Deface.NET.Configuration.Provider.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.System;

internal class FileSystem : IFileSystem
{
    public string BaseDirectory { get; private init; }

    public FileSystem(ISettingsProvider settingsProvider)
    {
        BaseDirectory = string.IsNullOrWhiteSpace(settingsProvider.Settings.CustomBaseDirectory)
            ? AppContext.BaseDirectory
            : settingsProvider.Settings.CustomBaseDirectory;
    }

    [ExcludeFromCodeCoverage]
    public FileStream OpenRead(string path)
    {
        return File.OpenRead(path);
    }

    [ExcludeFromCodeCoverage]
    public void Save(string path, byte[] data)
    {
        File.WriteAllBytes(path, data);
    }

    [ExcludeFromCodeCoverage]
    public string[] GetFiles(string path)
    {
        return Directory.GetFiles(path);
    }
}
