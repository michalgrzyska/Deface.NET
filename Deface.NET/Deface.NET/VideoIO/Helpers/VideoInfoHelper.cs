using Deface.NET.Common;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Helpers;

internal static class VideoInfoHelper
{
    public static VideoInfo ConvertOutputToVideoInfo(VideoInfoOutput output, string filePath)
    {
        var stream = output.Streams.FirstOrDefault() ?? throw new InvalidOperationException(ExceptionMessages.VideoHasNoStreams);

        var width = stream.Width;
        var height = stream.Height;
        var frames = int.Parse(stream.Frames);
        var targetFps = ParseFrameRateString(stream.TargetFrameRate);
        var averageFps = ParseFrameRateString(stream.AverageFrameRate);

        return new(width, height, frames, targetFps, averageFps, filePath);
    }

    public static float ParseFrameRateString(string str)
    {
        var numberParts = str.Split('/').Select(int.Parse).ToArray();
        var result = (float)numberParts[0] / numberParts[1];

        return result;
    }
}
