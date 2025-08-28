using Deface.NET.Common;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.ONNX;
using Microsoft.ML;
using SkiaSharp;

namespace Deface.NET.ObjectDetection.LicensePlates;

internal class LicensePlatesDetector : OnnxDetectorBase<Input, Output>, ILicensePlatesDetector
{
    private readonly PredictionEngine<Input, Output> _predictionEngine;

    private const int Width = 640;
    private const int Height = 640;
    private const float IouThreshold = 0.5f;
    private const int ParametersCount = 7;

    public LicensePlatesDetector(IOnnxProvider onnxProvider, ISettingsProvider settingsProvider, IAppFiles appFiles) 
        : base(onnxProvider, settingsProvider.Settings, appFiles.LicensePlatesONNX)
    {
        _predictionEngine = GetPredictionEngine();
    }

    public void Dispose() => _predictionEngine.Dispose();

    private PredictionEngine<Input, Output> GetPredictionEngine()
    {
        var model = _pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<Input>()));
        return _mlContext.Model.CreatePredictionEngine<Input, Output>(model);
    }

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var preprocessedImage = PreprocessImage(frame);
        Input input = new(preprocessedImage);

        var output = _predictionEngine.Predict(input);
        var result = PostProcess(output, frame.Width, frame.Height, settings.Threshold);

        return result;
    }

    private static float[] PreprocessImage(Frame frame)
    {
        var resized = (SKBitmap)frame.AsRescaledWithPadding(Width, Height);
        int size = 3 * Height * Width;
        var imageData = new float[size];

        unsafe
        {
            var pixels = (byte*)resized.GetPixels();
            int totalPixels = Height * Width;

            for (int i = 0; i < totalPixels; i++)
            {
                byte* pixel = pixels + i * 4;

                // No normalization, just cast to float
                imageData[i] = (float)pixel[0];               // channel 0
                imageData[i + totalPixels] = (float)pixel[1]; // channel 1
                imageData[i + totalPixels * 2] = (float)pixel[2]; // channel 2
            }
        }

        return imageData;
    }

    private static List<DetectedObject> PostProcess(Output output, int originalW, int originalH, float confidenceThreshold)
    {
        var results = new List<DetectedObject>();
        var numBoxes = output.Scores.Length;

        for (int i = 0; i < numBoxes; i++)
        {
            var score = output.Scores[i];

            if (score < confidenceThreshold)
            {
                continue;
            }

            var x1 = output.Boxes[i * 4 + 0];
            var y1 = output.Boxes[i * 4 + 1];
            var x2 = output.Boxes[i * 4 + 2];
            var y2 = output.Boxes[i * 4 + 3];

            results.Add(new DetectedObject((int)x1, (int)y1, (int)x2, (int)y2, score));
        }

        var boxesNms = NMS(results, IouThreshold);
        var rescaledBoxes = RescaleBoundingBoxes(boxesNms, originalW, originalH);

        return rescaledBoxes;
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
