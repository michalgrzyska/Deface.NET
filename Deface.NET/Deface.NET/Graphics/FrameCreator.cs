using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.System;

namespace Deface.NET.Graphics;

internal class FrameCreator(IFileSystem fileSystem) : IFrameCreator
{
    private readonly IFileSystem _fileSystem = fileSystem;

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
