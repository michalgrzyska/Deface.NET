using Deface.NET.Graphics.Interfaces;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics.Helpers;

internal static class GraphicsTestExtensions
{
    public static void ShouldBe(this IRgb pixel, byte r, byte g, byte b)
    {
        pixel.R.ShouldBe(r);
        pixel.G.ShouldBe(g);
        pixel.B.ShouldBe(b);
    }

    public static void ShouldBe(this IRgb pixel1, IRgb pixel2)
    {
        pixel1.R.ShouldBe(pixel2.R);
        pixel1.G.ShouldBe(pixel2.G);
        pixel1.B.ShouldBe(pixel2.B);
    }

    public static void ShouldBe(this SKColor pixel, byte r, byte g, byte b)
    {
        pixel.Red.ShouldBe(r);
        pixel.Green.ShouldBe(g);
        pixel.Blue.ShouldBe(b);
    }

    public static void ShouldBe(this ISize size, int width, int height)
    {
        size.Width.ShouldBe(width);
        size.Height.ShouldBe(height);
    }

    public static void ShouldBe(this SKBitmap bitmap, int width, int height)
    {
        bitmap.Width.ShouldBe(width);
        bitmap.Height.ShouldBe(height);
    }
}
