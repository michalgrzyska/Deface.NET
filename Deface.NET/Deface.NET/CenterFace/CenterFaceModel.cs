using OpenCvSharp;
using OpenCvSharp.Dnn;

namespace Deface.NET.CenterFace;

internal sealed class CenterFaceModel : IDisposable
{
    private readonly Net _net;

    public CenterFaceModel()
    {
        _net = CvDnn.ReadNetFromOnnx(AppFiles.CenterfaceONNX)!;
    }

    public List<FaceInfo> Detect(
        Mat image,
        Size resizedSize,
        float scoreThreshold = 0.5f,
        float nmsThreshold = 0.3f)
    {
        ArgumentNullException.ThrowIfNull(image);
        image.ThrowIfDisposed();

        if (image.Empty())
        {
            throw new ArgumentException($"{nameof(image)} is empty");
        }

        CenterFaceParams p = new(image, resizedSize.Width, resizedSize.Height, scoreThreshold, nmsThreshold);
        Size size = new(p.DW, p.DH);

        using Mat input = new();
        Cv2.Resize(image, input, size);

        using Mat blobInput = CvDnn.BlobFromImage(input, 1.0, size, new Scalar(0, 0, 0), true, false);
        _net.SetInput(blobInput, "input.1");

        using (Mat heatMap = new())
        using (Mat scale = new())
        using (Mat offset = new())
        using (Mat landmarks = new())
        {
            _net.Forward([heatMap, scale, offset, landmarks], ["537", "538", "539", "540"]);

            CenterFaceDecoder decoder = new(heatMap, scale, offset, landmarks, p);
            return decoder.GetOutput();
        }
    }

    public void Dispose() => _net.Dispose();
}
