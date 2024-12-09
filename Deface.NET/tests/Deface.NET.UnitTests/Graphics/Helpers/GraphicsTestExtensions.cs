using Deface.NET.Graphics.Interfaces;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics.Helpers;

internal static class GraphicsTestExtensions
{
    public static void ShouldBe(this IRgb pixel, byte r, byte g, byte b)
    {
        pixel.R.Should().Be(r);
        pixel.G.Should().Be(g);
        pixel.B.Should().Be(b);
    }

    public static void ShouldBe(this IRgb pixel1, IRgb pixel2)
    {
        pixel1.R.Should().Be(pixel2.R);
        pixel1.G.Should().Be(pixel2.G);
        pixel1.B.Should().Be(pixel2.B);
    }

    public static void ShouldBe(this SKColor pixel, byte r, byte g, byte b)
    {
        pixel.Red.Should().Be(r);
        pixel.Green.Should().Be(g);
        pixel.Blue.Should().Be(b);
    }

    public static void ShouldBe(this ISize size, int width, int height)
    {
        size.Width.Should().Be(width);
        size.Height.Should().Be(height);
    }

    public static void ShouldBe(this SKBitmap bitmap, int width, int height)
    {
        bitmap.Width.Should().Be(width);
        bitmap.Height.Should().Be(height);
    }
}
