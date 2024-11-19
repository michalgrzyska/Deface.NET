namespace Deface.NET.System;

internal interface IFileSystem
{
    FileStream OpenRead(string path);
    void Save(string path, byte[] data);
}