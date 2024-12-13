using Deface.NET.Graphics.Models;

namespace Deface.NET.ObjectDetection;

internal interface IDetector : IDisposable
{
    public List<DetectedObject> Detect(Frame frame, Settings settings);
}
