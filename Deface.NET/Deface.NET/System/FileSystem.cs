namespace Deface.NET.System;

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
}
