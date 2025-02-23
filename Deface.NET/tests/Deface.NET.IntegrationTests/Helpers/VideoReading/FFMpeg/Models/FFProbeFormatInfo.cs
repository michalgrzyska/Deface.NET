using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg.Models;

public class FFProbeFormatInfo
{
    [JsonPropertyName("size")]
    public string Size { get; set; }
}
