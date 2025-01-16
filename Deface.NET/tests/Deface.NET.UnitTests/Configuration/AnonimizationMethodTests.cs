using Deface.NET.Configuration;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Configuration;

public class AnonimizationMethodTests
{
    [Fact]
    public void GaussianBlur_ProperValuesAreSet()
    {
        var anonimizationMethod = AnonimizationMethod.GaussianBlur;
        anonimizationMethod.Type.ShouldBe(AnonimizationType.GaussianBlur);
    }

    [Fact]
    public void Mosaic_ProperValuesAreSet()
    {
        var anonimizationMethod = AnonimizationMethod.Mosaic;
        anonimizationMethod.Type.ShouldBe(AnonimizationType.Mosaic);
    }

    [Fact]
    public void Color_ProperEnumValueIsSet()
    {
        var anonimizationMethod = AnonimizationMethod.Color(0, 0, 0);
        anonimizationMethod.Type.ShouldBe(AnonimizationType.Color);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(255, 255, 255)]
    public void Color_ProperColorValuesAreSet(byte r, byte g, byte b)
    {
        var anonimizationMethod = AnonimizationMethod.Color(r, g, b);
        anonimizationMethod.ColorValue!.ShouldBe(r, g, b);
    }
}
