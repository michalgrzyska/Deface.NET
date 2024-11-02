using Newtonsoft.Json;

namespace Deface.NET.VideoIO.Models;

internal class VideoInfoOutput
{
    [JsonProperty("streams")]
    public VideoInfoStreamOutput[] Streams { get; set; } = [];
}

internal class VideoInfoStreamOutput
{
    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("nb_frames")]
    public string Frames { get; set; } = string.Empty;

    [JsonProperty("r_frame_rate")]
    public string TargetFrameRate { get; set; } = string.Empty;

    [JsonProperty("avg_frame_rate")]
    public string AverageFrameRate { get; set; } = string.Empty;
}