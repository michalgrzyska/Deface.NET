using Deface.NET.Graphics.Models;

namespace Deface.NET.UnitTests.Graphics.Helpers;

internal static class GraphicsTestExtensions
{
    public static void ShouldBe(this Pixel pixel, byte r, byte g, byte b)
    {
        pixel.R.Should().Be(r);
        pixel.G.Should().Be(g);
        pixel.B.Should().Be(b);
    }
}
