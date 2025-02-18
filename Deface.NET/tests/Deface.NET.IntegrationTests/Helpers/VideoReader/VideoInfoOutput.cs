using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.VideoReader;

internal class VideoInfoOutput
{
    [JsonPropertyName("streams")]
    public VideoInfoStreamOutput[] Streams { get; set; } = [];
}

internal class VideoInfoStreamOutput
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}
