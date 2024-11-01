using Newtonsoft.Json;
using System.Diagnostics;

namespace Deface.NET.VideoIO;

internal class VideoInfo
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private VideoInfo(int width, int height)
    {
        Width = width; 
        Height = height;
    }

    public static async Task<VideoInfo> GetInfo(string filePath)
    {
        using ExternalProcess process = new("ffprobe.exe", $"-v error -select_streams v:0 -show_entries stream=width,height -of json \"{filePath}\"");

        var output = await process.ExecuteWithOutput();
        var result = JsonConvert.DeserializeObject<Output>(output);

        return new(result.Streams[0].Width, result.Streams[0].Height);
    }
}

internal class Output
{
    [JsonProperty("streams")]
    public StreamOutput[] Streams { get; set; } = [];
}

internal class StreamOutput
{
    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }
}