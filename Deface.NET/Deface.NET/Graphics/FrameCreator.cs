using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.System;
using SkiaSharp;

namespace Deface.NET.Graphics;

internal class FrameCreator(IFileSystem fileSystem) : IFrameCreator
{
    private readonly IFileSystem _fileSystem = fileSystem;

    public Frame FromBgrArray(byte[] bgrData, int width, int height)
    {
        if (width * height * 3 != bgrData.Length)
        {
            throw new ArgumentException($"{nameof(bgrData)} length must be the size of width * height * 3");
        }

        byte[] rgbaData = GraphicsHelper.ConvertBgrToRgba(bgrData, width, height);
        SKBitmap bitmap = GraphicsHelper.GetBgraBitmapFromRawBytes(rgbaData, width, height);

        return (Frame)bitmap;
    }

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
