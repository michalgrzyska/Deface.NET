using Deface.NET.CenterFace;
using Deface.NET.Configuration;
using OpenCvSharp;

namespace Deface.NET.Processing;

internal class ShapeDrawingService(Settings settings)
{
    private readonly Settings _settings = settings;

    private const double MosaicSizeFactor = 0.03;
    private const double GaussianBlurFactor = 0.1;

    public void DrawShapes(Mat frame, IEnumerable<FaceInfo> faces)
    {
        if (_settings.MaskScale != 1.0f)
        {
            foreach (FaceInfo face in faces)
            {
                face.Enlarge(_settings.MaskScale);
            }
        }

        Action<Mat, FaceInfo> processFn = _settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => DrawColorShape,
            AnonimizationType.GaussianBlur => DrawGaussianBlurShape,
            AnonimizationType.Mosaic => DrawMosaicShape,
            _ => throw new NotImplementedException()
        };

        foreach (var face in faces)
        {
            processFn(frame, face);
        }
    }

    private void DrawColorShape(Mat frame, FaceInfo face)
    {
        var color = _settings.AnonimizationMethod.ColorValue!.Value;

        if (_settings.AnonimizationShape == AnonimizationShape.Ellipse)
        {
            var (center, axes) = face.ToEllipse();
            Cv2.Ellipse(frame, center, axes, angle: 0, startAngle: 0, endAngle: 360, color, thickness: -1);

        }
        else if (_settings.AnonimizationShape == AnonimizationShape.Rectangle)
        {
            var rect = face.ToRect();
            Cv2.Rectangle(frame, rect, color, thickness: -1);
        }
    }

    private void DrawGaussianBlurShape(Mat frame, FaceInfo face)
    {
        var factor = (int)(frame.Width > frame.Height
            ? frame.Width * GaussianBlurFactor
            : frame.Height * GaussianBlurFactor);

        var factorNormalized = factor % 2 == 0 ? factor + 1 : factor; // Blur size must be odd number

        Size gaussianBlurSize = new(factorNormalized, factorNormalized);

        if (_settings.AnonimizationShape == AnonimizationShape.Rectangle)
        {
            var rect = face.ToRect();
            Mat roi = new(frame, rect);

            Cv2.GaussianBlur(roi, roi, gaussianBlurSize, sigmaX: 0);
            roi.CopyTo(frame[rect]);
        }
        else if (_settings.AnonimizationShape == AnonimizationShape.Ellipse)
        {
            var (center, axes) = face.ToEllipse();

            Mat blurredImage = new();
            Cv2.GaussianBlur(frame, blurredImage, gaussianBlurSize, sigmaX: 0);

            Mat mask = Mat.Zeros(frame.Size(), MatType.CV_8UC1);
            Cv2.Ellipse(mask, center, axes, angle: 0, startAngle: 0, endAngle: 360, color: new Scalar(255), thickness: -1);

            blurredImage.CopyTo(frame, mask);
        }
    }

    private void DrawMosaicShape(Mat frame, FaceInfo face)
    {
        var mosaicSize = (int)(frame.Width > frame.Height
            ? frame.Width * MosaicSizeFactor
            : frame.Height * MosaicSizeFactor);

        for (int y = (int)face.Y1; y < face.Y2; y += mosaicSize)
        {
            for (int x = (int)face.X1; x < face.X2; x += mosaicSize)
            {
                Point pt1 = new(x, y);
                Point pt2 = new(Math.Min((int)face.X2, x + mosaicSize - 1), Math.Min(face.Y2, y + mosaicSize - 1));

                Vec3b pixelColor = frame.At<Vec3b>(y, x); // BGR format
                Scalar color = new(pixelColor[0], pixelColor[1], pixelColor[2]);

                Cv2.Rectangle(frame, pt1, pt2, new Scalar(color[0], color[1], color[2]), -1);
            }
        }
    }
}