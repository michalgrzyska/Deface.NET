namespace Deface.NET.Processing;

internal class FileSystem
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
