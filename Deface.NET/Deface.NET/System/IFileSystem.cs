﻿namespace Deface.NET.System;

internal interface IFileSystem
{
    string[] GetFiles(string path);
    FileStream OpenRead(string path);
    void Save(string path, byte[] data);
}