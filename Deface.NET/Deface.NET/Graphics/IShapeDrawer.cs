using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics;

internal interface IShapeDrawer
{
    Frame Draw(Frame bitmap, List<DetectedObject> objects, Settings settings);
}
