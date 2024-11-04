using Deface.NET.Graphics;

namespace Deface.NET.ObjectDetection;

internal interface IDetector
{
    public List<DetectedObject> Detect(Frame frame, float threshold);
}
