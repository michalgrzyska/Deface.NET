using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics;

internal interface IDrawer
{
    SKBitmap Draw(SKBitmap bitmap, List<DetectedObject> objects, Settings settings);
}
