using Deface.NET.CommercialFeatures.Interfaces;

namespace Deface.NET.CommercialFeatures.Features;

internal class H264CommercialFeature : ICommercialFeature
{
    public string Name => "H.264 Commercial License";

    public string[] Urls =>
    [
        "https://www.via-la.com/licensing-2/avc-h-264/avc-h-264-license-fees",
        "https://x264.org/licensing"
    ];

    public bool IsEnabled(Settings settings)
    {
        return settings.EncodingCodec == EncodingCodec.H264;
    }
}
