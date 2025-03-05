using Deface.NET.Common;
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
        using var stream = _fileSystem.OpenRead(path);
        var bitmap = SKBitmap.Decode(stream);

        if (bitmap == null)
        {
            throw new InvalidOperationException(string.Format(ExceptionMessages.FailedToDecodeImage, path));
        }

        return (Frame)bitmap;
    }
}
