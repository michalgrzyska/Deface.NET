using Deface.NET.IntegrationTests.Config;
using System.Diagnostics;
using System.Text.Json;

namespace Deface.NET.IntegrationTests.Helpers.Models;

internal class TestVideo
{
    public static VideoInfo GetVideoInfo(string videoPath)
    {
        var ffprobePath = ExecutablePath.FFProbe;

        var arguments = string.Join(" ",
            "-v error",
            "-select_streams v:0",
            "-show_entries stream=width,height,nb_frames",
            "-show_entries format=size",
            "-print_format json",
            $"\"{videoPath}\""
        );

        ProcessStartInfo info = new()
        {
            FileName = ffprobePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = new() { StartInfo = info };
        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        var videoInfo = JsonSerializer.Deserialize<FFProbeOutput>(output);
        var stream = videoInfo.Streams.FirstOrDefault();

        return new VideoInfo
        {
            Width = stream.Width,
            Height = stream.Height,
            TotalFrames = int.TryParse(stream.NbFrames, out int frames) ? frames : 0,
            FileSize = videoInfo.Format.Size
        };
    }
}
