using Deface.NET.CommercialFeatures.Interfaces;

namespace Deface.NET.UnitTests.CommercialFeatures;

public abstract class ICommercialFeatureBaseTests
{
    internal abstract Settings EnabledSettings { get; }
    internal abstract Settings DisabledSettings { get; }

    internal abstract ICommercialFeature GetCommercialFeature();

    [Fact]
    public void Name_IsNotNullOrEmpty()
    {
        // Arrange
        var commercialFeature = GetCommercialFeature();

        // Act
        var name = commercialFeature.Name;

        // Assert
        name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Urls_IsNotNullOrEmpty()
    {
        // Arrange
        var commercialFeature = GetCommercialFeature();

        // Act
        var urls = commercialFeature.Urls;

        // Assert
        urls.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void IsEnabled_WithEnabledSettings_ReturnsTrue()
    {
        // Arrange
        var commercialFeature = GetCommercialFeature();

        // Act
        var isEnabled = commercialFeature.IsEnabled(EnabledSettings);

        // Assert
        isEnabled.Should().BeTrue();
    }

    [Fact]
    public void IsEnabled_WithDisabledSettings_ReturnsFalse()
    {
        // Arrange
        var commercialFeature = GetCommercialFeature();

        // Act
        var isEnabled = commercialFeature.IsEnabled(DisabledSettings);

        // Assert
        isEnabled.Should().BeFalse();
    }
}
