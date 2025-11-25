using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.YoloNasLicensePlates;

namespace Deface.NET.ObjectDetection;

/// <summary>
/// This is the foundation for multiple models if needed (which is planned).
/// </summary>
internal class ObjectDetector(ILicensePlateDetector licensePlatesDetector) : IObjectDetector
{
    private readonly ILicensePlateDetector _licensePlatesDetector = licensePlatesDetector;

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var objects = _licensePlatesDetector.Detect(frame, settings);
        return objects.Select(x => x.GetResized(settings.MaskScale)).ToList();
    }
}
