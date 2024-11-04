using Deface.NET.Graphics;

namespace Deface.NET.ObjectDetection;

internal interface IObjectDetector
{
    public List<DetectedObject> Detect(Frame frame, float threshold);
}
