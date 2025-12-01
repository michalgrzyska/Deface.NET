using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.UltraFace;
using Deface.NET.ObjectDetection.YoloNasLicensePlates;

namespace Deface.NET.ObjectDetection;

/// <summary>
/// This is the foundation for multiple models if needed (which is planned).
/// </summary>
internal class ObjectDetector(ILicensePlateDetector licensePlatesDetector, IUltraFaceDetector ultraFaceDetector) : IObjectDetector
{
    private readonly ILicensePlateDetector _licensePlatesDetector = licensePlatesDetector;
    private readonly IUltraFaceDetector _ultraFaceDetector = ultraFaceDetector;

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var plates = _licensePlatesDetector.Detect(frame, settings);
        var faces = _ultraFaceDetector.Detect(frame, settings);

        List<DetectedObject> objects = [..plates, ..faces];
        return objects.Select(x => x.GetResized(settings.MaskScale)).ToList();
    }
}
