namespace Deface.NET.CommercialFeatures.Interfaces;

internal interface ICommercialFeature
{
    string Name { get; }
    bool IsEnabled(Settings settings);
    string[] Urls { get; }
}