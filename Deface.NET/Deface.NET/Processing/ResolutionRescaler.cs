using OpenCvSharp;

namespace Deface.NET.Processing;

internal static class ResolutionRescaler
{
    public static Size RescaleIfNeeded(VideoCapture videoCapture, int targetShortedSideLength)
    {
        var shorterSide = videoCapture.FrameWidth < videoCapture.FrameHeight
            ? videoCapture.FrameWidth
            : videoCapture.FrameHeight;

        if (shorterSide <= targetShortedSideLength)
        {
            return new(videoCapture.FrameWidth, videoCapture.FrameHeight);
        }

        return Rescale(videoCapture.FrameWidth, videoCapture.FrameHeight, targetShortedSideLength);
    }

    private static Size Rescale(int w, int h, int targetShortedSideLength)
    {
        var ratio = (double)w / h;

        if (w < h)
        {
            var targetW = targetShortedSideLength;
            var targetH = Math.Round(targetW / ratio);

            return new(targetW, (int)targetH);
        }
        else if (w > h)
        {
            var targetH = targetShortedSideLength;
            var targetW = Math.Round(targetH * ratio);

            return new((int)targetW, targetH);
        }
        else // 1:1 (square) video
        {
            return new(targetShortedSideLength, targetShortedSideLength);
        }
    }
}
