namespace Deface.NET.System;

internal interface IFileSystem
{
    string BaseDirectory { get; }

    string[] GetFiles(string path);
    FileStream OpenRead(string path);
    void Save(string path, byte[] data);
    bool Exists(string path);
}