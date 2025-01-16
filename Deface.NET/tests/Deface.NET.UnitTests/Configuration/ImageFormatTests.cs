using SkiaSharp;

namespace Deface.NET.UnitTests.Configuration;

public class ImageFormatTests
{
    [Fact]
    public void Png_HasProperDataSet()
    {
        var imageFormat = ImageFormat.Png;

        imageFormat.Quality.ShouldBe(100);
        imageFormat.Format.ShouldBe(SKEncodedImageFormat.Png);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(55)]
    [InlineData(77)]
    public void Jpeg_WithQuality_HasProperDataSet(int quality)
    {
        var imageFormat = ImageFormat.Jpeg(quality);

        imageFormat.Quality.ShouldBe(quality);
        imageFormat.Format.ShouldBe(SKEncodedImageFormat.Jpeg);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(101)]
    [InlineData(200)]
    public void Jpeg_InvalidQuality_ThrowsArgumentOutOfRangeException(int quality)
    {
        var imageFormat = ImageFormat.Jpeg(quality);

        var action = () => imageFormat.Validate();

        action.ShouldThrow<ArgumentOutOfRangeException>();
    }
}
