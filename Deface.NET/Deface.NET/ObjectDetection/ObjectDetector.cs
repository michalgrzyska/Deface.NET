using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.LicensePlates;
using Deface.NET.ObjectDetection.UltraFace;

namespace Deface.NET.ObjectDetection;

/// <summary>
/// This is the foundation for multiple models if needed (which is planned).
/// </summary>
internal class ObjectDetector(IUltraFaceDetector ultraFaceDetector, ILicensePlatesDetector licensePlatesDetector) : IObjectDetector
{
    private readonly IUltraFaceDetector _ultraFaceDetector = ultraFaceDetector;
    private readonly ILicensePlatesDetector _licensePlatesDetector = licensePlatesDetector;

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        //var objects = _ultraFaceDetector.Detect(frame, settings);
        var objects = _licensePlatesDetector.Detect(frame, settings);
        return objects.Select(x => x.GetResized(settings.MaskScale)).ToList();
    }
}
