using Deface.NET.Configuration;
using FluentAssertions;

namespace Deface.NET.UnitTests.SettingsTests;

public class AnonimizationMethodUnitTests
{
    [Fact]
    public void GaussianBlur_ProperValuesAreSet()
    {
        AnonimizationMethod anonimizationMethod = AnonimizationMethod.GaussianBlur;
        anonimizationMethod.Type.Should().Be(AnonimizationType.GaussianBlur);
    }

    [Fact]
    public void Mosaic_ProperValuesAreSet()
    {
        AnonimizationMethod anonimizationMethod = AnonimizationMethod.Mosaic;
        anonimizationMethod.Type.Should().Be(AnonimizationType.Mosaic);
    }

    [Fact]
    public void Color_ProperEnumValueIsSet()
    {
        AnonimizationMethod anonimizationMethod = AnonimizationMethod.Color(0, 0, 0);
        anonimizationMethod.Type.Should().Be(AnonimizationType.Color);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(255, 255, 255)]
    public void Color_ProperColorValuesAreSet(byte r, byte g, byte b)
    {
        AnonimizationMethod anonimizationMethod = AnonimizationMethod.Color(r, g, b);

        anonimizationMethod.ColorValue!.R.Should().Be(r);
        anonimizationMethod.ColorValue!.G.Should().Be(g);
        anonimizationMethod.ColorValue!.B.Should().Be(b);
    }
}
