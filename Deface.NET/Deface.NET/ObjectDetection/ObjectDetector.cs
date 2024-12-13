using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.UltraFace;

namespace Deface.NET.ObjectDetection;

/// <summary>
/// This is the foundation for multiple models if needed (which is planned).
/// </summary>
internal class ObjectDetector(IUltraFaceDetector ultraFaceDetector) : IObjectDetector
{
    private readonly IUltraFaceDetector _ultraFaceDetector = ultraFaceDetector;

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var objects = _ultraFaceDetector.Detect(frame, settings);
        return objects.Select(x => x.GetResized(settings.MaskScale)).ToList();
    }
}
