using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg.Models;

public class FFProbeOutput
{
    [JsonPropertyName("streams")]
    public List<FFProbeStreamInfo> Streams { get; set; }

    [JsonPropertyName("format")]
    public FFProbeFormatInfo Format { get; set; }
}
