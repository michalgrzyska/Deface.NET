﻿using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Processing;

namespace Deface.NET.Graphics;

internal class FrameCreator(FileSystem fileSystem) : IFrameCreator
{
    private readonly FileSystem _fileSystem = fileSystem;

    public Frame FromFile(string path)
    {
        try
        {
            using FileStream stream = _fileSystem.OpenRead(path);
            return new(stream);
        }
        catch (Exception e)
        {
            throw new DefaceException($"Could not open image {path}.", e);
        }
    }
}
