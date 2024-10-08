using Deface.NET.CommercialFeatures.Features;
using Deface.NET.CommercialFeatures.Interfaces;

namespace Deface.NET.CommercialFeatures;

internal static class CommercialFeatures
{
    private static List<ICommercialFeature> _features = [];
    public static IReadOnlyCollection<ICommercialFeature> Features => _features;

    static CommercialFeatures()
    {
        Register<H264CommercialFeature>();
    }

    private static void Register<T>() where T : ICommercialFeature, new()
    {
        var feature = new T();
        _features.Add(feature);
    }
}
