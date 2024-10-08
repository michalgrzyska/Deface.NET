using OpenCvSharp;

namespace Deface.NET;

internal static class Extensions
{
    public static Size ToSize(this VideoCapture videoCapture) => new(videoCapture.FrameWidth, videoCapture.FrameHeight);

    public static Dimensions ToDimensions(this Size size) => new(size.Width, size.Height);
}
