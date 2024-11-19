using Deface.NET.Graphics.Models;

namespace Deface.NET.ObjectDetection;

internal interface IObjectDetector : IDisposable
{
    List<DetectedObject> Detect(Frame frame, Settings settings);
}