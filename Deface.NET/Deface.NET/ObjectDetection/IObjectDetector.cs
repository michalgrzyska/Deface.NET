using SkiaSharp;

namespace Deface.NET.ObjectDetection;

internal interface IObjectDetector
{
    public List<DetectedObject> Detect(SKBitmap bitmap);
}
