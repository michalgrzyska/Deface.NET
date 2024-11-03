using Deface.NET.Graphics;
using Deface.NET.Utils;
using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;

namespace Deface.NET.ObjectDetection.UltraFace;

internal class UltraFaceDetector : IObjectDetector, IDisposable
{
    private readonly MLContext _mlContext;
    private readonly OnnxScoringEstimator _pipeline;
    private readonly PredictionEngine<Input, Output> _predictionEngine;

    private const int Width = 640;
    private const int Height = 480;
    private const float IouThreshold = 0.5f;

    public UltraFaceDetector()
    {
        _mlContext = new();
        _pipeline = GetPipeline();
        _predictionEngine = GetPredictionEngine();
    }

    public List<DetectedObject> Detect(Frame frame)
    {
        float[] preprocessedImage = PreprocessImage(frame);
        Input input = new(preprocessedImage);
        Output output = _predictionEngine.Predict(input);

        return PostProcess(output.Scores, output.Boxes, frame.Width, frame.Height);
    }

    public void Dispose() => _predictionEngine.Dispose();

    private OnnxScoringEstimator GetPipeline()
    {
        return _mlContext.Transforms.ApplyOnnxModel(
            modelFile: AppFiles.UltraFaceONNX,
            outputColumnNames: ["scores", "boxes"],
            inputColumnNames: ["input"]
        );
    }

    private PredictionEngine<Input, Output> GetPredictionEngine()
    {
        OnnxTransformer model = _pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<Input>()));
        return _mlContext.Model.CreatePredictionEngine<Input, Output>(model);
    }

    private static float[] PreprocessImage(Frame frame)
    {
        Frame resized = frame.GetRescaledWithPadding(Width, Height);
        float[] imageData = new float[1 * 3 * Height * Width];
        int index = 0;

        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                Pixel pixel = resized.GetPixel(x, y);

                imageData[index] = (pixel.R - 127) / 128f;
                imageData[index + Height * Width] = (pixel.G - 127) / 128f;
                imageData[index + 2 * Height * Width] = (pixel.B - 127) / 128f;

                index++;
            }
        }

        return imageData;
    }

    private static List<DetectedObject> PostProcess(float[] scores, float[] boxes, int originalW, int originalH, float confidenceThreshold = 0.5f)
    {
        List<DetectedObject> faces = [];
        int numBoxes = scores.Length / 2;

        for (int i = 0; i < numBoxes; i++)
        {
            float score = scores[i * 2 + 1];

            if (score > confidenceThreshold)
            {
                float x1 = boxes[i * 4] * Width;
                float y1 = boxes[i * 4 + 1] * Height;
                float x2 = boxes[i * 4 + 2] * Width;
                float y2 = boxes[i * 4 + 3] * Height;

                faces.Add(new((int)x1, (int)y1, (int)x2, (int)y2, score));
            }
        }

        var boxesNms = NMS(faces, IouThreshold);
        var rescaledBoxes = RescaleBoundingBoxes(boxesNms, originalW, originalH);

        return rescaledBoxes;
    }

    private static List<DetectedObject> NMS(List<DetectedObject> faces, float iouThreshold)
    {
        List<DetectedObject> sortedFaces = faces.OrderByDescending(f => f.Confidence).ToList();
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
        int intersectionX1 = Math.Max(box1.X1, box2.X1);
        int intersectionY1 = Math.Max(box1.Y1, box2.Y1);
        int intersectionX2 = Math.Min(box1.X2, box2.X2);
        int intersectionY2 = Math.Min(box1.Y2, box2.Y2);

        int intersectionArea = Math.Max(0, intersectionX2 - intersectionX1 + 1) * Math.Max(0, intersectionY2 - intersectionY1 + 1);

        int box1Area = (box1.X2 - box1.X1 + 1) * (box1.Y2 - box1.Y1 + 1);
        int box2Area = (box2.X2 - box2.X1 + 1) * (box2.Y2 - box2.Y1 + 1);
        int unionArea = box1Area + box2Area - intersectionArea;

        return (float)intersectionArea / unionArea;
    }

    private static List<DetectedObject> RescaleBoundingBoxes(List<DetectedObject> scaledBoxes, int originalWidth, int originalHeight)
    {
        float scale = Math.Min((float)Width / originalWidth, (float)Height / originalHeight);

        int offsetX = (Width - (int)(originalWidth * scale)) / 2;
        int offsetY = (Height - (int)(originalHeight * scale)) / 2;

        List<DetectedObject> originalBoxes = [];

        foreach (var box in scaledBoxes)
        {
            float x1 = (box.X1 - offsetX) / scale;
            float y1 = (box.Y1 - offsetY) / scale;
            float x2 = (box.X2 - offsetX) / scale;
            float y2 = (box.Y2 - offsetY) / scale;

            originalBoxes.Add(new DetectedObject((int)x1, (int)y1, (int)x2, (int)y2, box.Confidence));
        }

        return originalBoxes;
    }
}