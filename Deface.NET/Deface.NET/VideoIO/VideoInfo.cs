using Deface.NET.VideoIO.Models;
using Newtonsoft.Json;

namespace Deface.NET.VideoIO;

internal record VideoInfo(int Width, int Height, int TotalFrames)
{
    public static async Task<VideoInfo> GetInfo(string filePath)
    {
        using ExternalProcess process = new(
            "ffprobe.exe",
            $"-v error -select_streams v:0 -show_entries stream=width,height,nb_frames,r_frame_rate,avg_frame_rate -of json \"{filePath}\""
        );

        var output = await process.ExecuteWithOutput();
        var result = JsonConvert.DeserializeObject<VideoInfoOutput>(output)!;
        var stream = result.Streams[0];

        return new(stream.Width, stream.Height, int.Parse(stream.Frames));
    }


}