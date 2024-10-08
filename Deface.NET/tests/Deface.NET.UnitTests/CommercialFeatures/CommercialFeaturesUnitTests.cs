using Deface.NET.CommercialFeatures.Interfaces;
using Shouldly;

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

        iCommercialFeaturesImplementations.Count.ShouldBe(NET.CommercialFeatures.CommercialFeatures.Features.Count);
    }
}
