using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.UltraFace;

namespace Deface.NET.ObjectDetection;

/// <summary>
/// This is the foundation for multiple models if needed (which is planned).
/// </summary>
internal class ObjectDetector : IObjectDetector
{
    private UltraFaceDetector _ultraface = default!;
    private UltraFaceDetector Ultraface => _ultraface ??= new();

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var objects = Ultraface.Detect(frame, settings.Threshold);
        return objects.Select(x => x.GetResized(settings.MaskScale)).ToList();
    }

    public void Dispose()
    {
        _ultraface?.Dispose();
    }
}
