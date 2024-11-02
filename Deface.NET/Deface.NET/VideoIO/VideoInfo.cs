using Deface.NET.VideoIO.Models;
using Newtonsoft.Json;

namespace Deface.NET.VideoIO;

internal record VideoInfo(int Width, int Height)
{
    public static async Task<VideoInfo> GetInfo(string filePath)
    {
        using ExternalProcess process = new(
            "ffprobe.exe",
            $"-v error -select_streams v:0 -show_entries stream=width,height -of json \"{filePath}\""
        );

        var output = await process.ExecuteWithOutput();
        var result = JsonConvert.DeserializeObject<VideoInfoOutput>(output)!;

        return new(result.Streams[0].Width, result.Streams[0].Height);
    }
}