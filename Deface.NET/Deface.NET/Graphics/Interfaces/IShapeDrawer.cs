using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics.Interfaces;

internal interface IShapeDrawer
{
    Frame DrawShapes(Frame frame, List<DetectedObject> objects);
}
