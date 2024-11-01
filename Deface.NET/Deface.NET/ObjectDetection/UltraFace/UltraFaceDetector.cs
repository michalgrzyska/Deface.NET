using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;
using SkiaSharp;

namespace Deface.NET.ObjectDetection.UltraFace;

internal class UltraFaceDetector : IObjectDetector
{
    private readonly MLContext mlContext;
    private readonly OnnxScoringEstimator pipeline;
    private readonly PredictionEngine<Input, Output> predictionEngine;

    private const int Width = 640;
    private const int Height = 480;

    public UltraFaceDetector()
    {
        mlContext = new();
        pipeline = GetPipeline();
        predictionEngine = GetPredictionEngine();
    }

    public List<DetectedObject> Detect(SKBitmap bitmap)
    {
        float[] preprocessedImage = PreprocessImage(bitmap);
        Input input = new(preprocessedImage);
        Output output = predictionEngine.Predict(input);

        return PostProcess(output.Scores, output.Boxes);
    }

    private OnnxScoringEstimator GetPipeline()
    {
        return mlContext.Transforms.ApplyOnnxModel(
            modelFile: AppFiles.UltraFaceONNX,
            outputColumnNames: ["scores", "boxes"],
            inputColumnNames: ["input"]
        );
    }

    private PredictionEngine<Input, Output> GetPredictionEngine()
    {
        OnnxTransformer model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<Input>()));
        return mlContext.Model.CreatePredictionEngine<Input, Output>(model);
    }

    private static float[] PreprocessImage(SKBitmap bitmap)
    {
        SKBitmap resized = bitmap.Resize(new SKSizeI(Width, Height), SKFilterQuality.High);
        float[] imageData = new float[1 * 3 * Height * Width];
        int index = 0;

        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                SKColor pixel = resized.GetPixel(x, y);

                imageData[index] = (pixel.Red - 127) / 128f;
                imageData[index + Height * Width] = (pixel.Green - 127) / 128f;
                imageData[index + 2 * Height * Width] = (pixel.Blue - 127) / 128f;

                index++;
            }
        }

        return imageData;
    }

    private static List<DetectedObject> PostProcess(float[] scores, float[] boxes, float confidenceThreshold = 0.5f, float iouThreshold = 0.5f)
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

        return NMS(faces, iouThreshold);
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
}