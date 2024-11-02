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
}