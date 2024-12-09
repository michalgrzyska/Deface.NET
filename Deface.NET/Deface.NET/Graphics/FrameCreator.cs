using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.System;
using SkiaSharp;

namespace Deface.NET.Graphics;

internal class FrameCreator(IFileSystem fileSystem) : IFrameCreator
{
    private readonly IFileSystem _fileSystem = fileSystem;

    public Frame FromBgraArray(byte[] bgraData, int width, int height)
    {
        var bitmap = GraphicsHelper.CreateBitmapFromBgra(bgraData, width, height);
        return (Frame)bitmap;
    }

    public Frame FromFile(string path)
    {
        try
        {
            using var stream = _fileSystem.OpenRead(path);
            var bitmap = SKBitmap.Decode(stream);

            return (Frame)bitmap;
        }
        catch (Exception e)
        {
            throw new DefaceException($"Could not open image {path}.", e);
        }
    }
}
