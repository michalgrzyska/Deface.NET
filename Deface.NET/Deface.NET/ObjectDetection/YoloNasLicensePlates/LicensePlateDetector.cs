using Deface.NET.Common;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.ONNX;
using Microsoft.ML;
using SkiaSharp;

namespace Deface.NET.ObjectDetection.YoloNasLicensePlates;

internal class LicensePlateDetector : OnnxDetectorBase<Input, Output>, ILicensePlateDetector
{
    private readonly PredictionEngine<Input, Output> _predictionEngine;

    private const int Width = 640;
    private const int Height = 640;
    private const float IouThreshold = 0.4f;
    private const int NumAnchors = 8400;
    private const int BoxStride = 4;

    // GpuDeviceId goes from global settings
    public LicensePlateDetector(IOnnxProvider onnxProvider, ISettingsProvider settingsProvider, IAppFiles appFiles) 
        : base(onnxProvider, settingsProvider.Settings, appFiles.LicensePlatesONNX)
    {
        _predictionEngine = GetPredictionEngine();
    }

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var preprocessedImage = PreprocessImage(frame);
        Input input = new(preprocessedImage);

        var output = _predictionEngine.Predict(input);
        var result = PostProcess(output, frame.Width, frame.Height, settings.LicensePlateThreshold);

        return result;
    }

    public void Dispose() => _predictionEngine.Dispose();

    private PredictionEngine<Input, Output> GetPredictionEngine()
    {
        var model = _pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<Input>()));
        return _mlContext.Model.CreatePredictionEngine<Input, Output>(model);
    }
    private static float[] PreprocessImage(Frame frame)
    {
        var resized = (SKBitmap)frame.AsRescaledWithPadding(Width, Height);
        float[] imageData = new float[1 * 3 * Height * Width];

        unsafe
        {
            var pixels = (byte*)resized.GetPixels();
            int total = Width * Height;

            int offsetR = 0;
            int offsetG = total;
            int offsetB = 2 * total;

            for (int i = 0; i < total; i++)
            {
                byte* p = pixels + i * 4; // BGRA memory

                imageData[offsetR + i] = p[2] / 255f; // R
                imageData[offsetG + i] = p[1] / 255f; // G
                imageData[offsetB + i] = p[0] / 255f; // B
            }
        }

        return imageData;
    }

    private static List<DetectedObject> PostProcess(Output output, int originalW, int originalH, float confThreshold)
    {
        float[] rawBoxes = output.Boxes;
        float[] rawScores = output.Scores;

        var detections = new List<DetectedObject>();

        for (int i = 0; i < NumAnchors; i++)
        {
            float score = rawScores[i];

            if (score < confThreshold)
                continue;

            int b = i * BoxStride;

            float x1 = rawBoxes[b + 0];
            float y1 = rawBoxes[b + 1];
            float x2 = rawBoxes[b + 2];
            float y2 = rawBoxes[b + 3];

            detections.Add(new DetectedObject(x1, y1, x2, y2, score));
        }

        detections = NMS(detections, 0.45f);
        return RescaleBoundingBoxes(detections, originalW, originalH);
    }

    private static List<DetectedObject> NMS(List<DetectedObject> faces, float iouThreshold)
    {
        var sortedFaces = faces.OrderByDescending(f => f.Confidence).ToList();
        List<DetectedObject> selectedFaces = [];

        while (sortedFaces.Count != 0)
        {
            var bestFace = sortedFaces.First();

            selectedFaces.Add(bestFace);
            sortedFaces.RemoveAt(0);
            sortedFaces.RemoveAll(f => IOU(bestFace, f) > iouThreshold);
        }

        return selectedFaces;
    }

    private static float IOU(DetectedObject box1, DetectedObject box2)
    {
        var intersectionX1 = Math.Max(box1.X1, box2.X1);
        var intersectionY1 = Math.Max(box1.Y1, box2.Y1);
        var intersectionX2 = Math.Min(box1.X2, box2.X2);
        var intersectionY2 = Math.Min(box1.Y2, box2.Y2);

        var intersectionArea = Math.Max(0, intersectionX2 - intersectionX1 + 1) * Math.Max(0, intersectionY2 - intersectionY1 + 1);

        var box1Area = (box1.X2 - box1.X1 + 1) * (box1.Y2 - box1.Y1 + 1);
        var box2Area = (box2.X2 - box2.X1 + 1) * (box2.Y2 - box2.Y1 + 1);
        var unionArea = box1Area + box2Area - intersectionArea;

        return (float)intersectionArea / unionArea;
    }
    private static List<DetectedObject> RescaleBoundingBoxes(List<DetectedObject> scaledBoxes, int originalWidth, int originalHeight)
    {
        var scale = Math.Min((float)Width / originalWidth, (float)Height / originalHeight);

        var offsetX = (Width - (int)(originalWidth * scale)) / 2;
        var offsetY = (Height - (int)(originalHeight * scale)) / 2;

        List<DetectedObject> originalBoxes = [];

        foreach (var box in scaledBoxes)
        {
            var x1 = (box.X1 - offsetX) / scale;
            var y1 = (box.Y1 - offsetY) / scale;
            var x2 = (box.X2 - offsetX) / scale;
            var y2 = (box.Y2 - offsetY) / scale;

            originalBoxes.Add(new((int)x1, (int)y1, (int)x2, (int)y2, box.Confidence));
        }

        return originalBoxes;
    }
}