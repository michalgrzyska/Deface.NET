using System.Text.Json.Serialization;

namespace Deface.NET.VideoIO.Models;

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

    [JsonPropertyName("nb_frames")]
    public string Frames { get; set; } = string.Empty;

    [JsonPropertyName("r_frame_rate")]
    public string TargetFrameRate { get; set; } = string.Empty;

    [JsonPropertyName("avg_frame_rate")]
    public string AverageFrameRate { get; set; } = string.Empty;
}