using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.Models;

public class FFProbeOutput
{
    [JsonPropertyName("streams")]
    public List<StreamInfo> Streams { get; set; }

    [JsonPropertyName("format")]
    public FormatInfo Format { get; set; }
}
