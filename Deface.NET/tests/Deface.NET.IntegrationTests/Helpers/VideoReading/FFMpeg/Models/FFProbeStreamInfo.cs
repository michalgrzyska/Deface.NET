using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading.FFMpeg.Models;

public class FFProbeStreamInfo
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("nb_frames")]
    public string NbFrames { get; set; }

    [JsonPropertyName("avg_frame_rate")]
    public string AverageFrameRate { get; set; } = string.Empty;
}
