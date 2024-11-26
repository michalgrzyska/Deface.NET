using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.System;

[ExcludeFromCodeCoverage]
internal class FileSystem : IFileSystem
{
    public FileStream OpenRead(string path)
    {
        return File.OpenRead(path);
    }

    public void Save(string path, byte[] data)
    {
        File.WriteAllBytes(path, data);
    }

    public string[] GetFiles(string path)
    {
        return Directory.GetFiles(path);
    }
}
