using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.Models;

public class StreamInfo
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("nb_frames")]
    public string NbFrames { get; set; }
}
