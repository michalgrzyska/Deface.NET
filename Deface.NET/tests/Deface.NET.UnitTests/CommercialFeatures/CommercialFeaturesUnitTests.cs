using Deface.NET.CommercialFeatures.Interfaces;

namespace Deface.NET.UnitTests.CommercialFeatures;

public class CommercialFeaturesUnitTests
{
    [Fact]
    public void EveryICommercialFeatureShouldBePresentInCommercialFeaturesCtor()
    {
        var assembly = typeof(ICommercialFeature).Assembly;

        var iCommercialFeaturesImplementations = assembly
            .GetTypes()
            .Where(t => typeof(ICommercialFeature).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        iCommercialFeaturesImplementations.Count.Should().Be(NET.CommercialFeatures.CommercialFeatures.Features.Count);
    }
}
