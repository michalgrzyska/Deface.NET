using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using SkiaSharp;

namespace UltrafaceConcept;

internal class Ultraface
{
    private MLContext mlContext;
    private OnnxScoringEstimator pipeline;
    private PredictionEngine<ModelInput, ModelOutput> predictionEngine;

    private const int Width = 640;
    private const int Height = 480;

    public Ultraface()
    {
        mlContext = new MLContext();
        mlContext.Log += MlContext_Log;

        pipeline = mlContext.Transforms.ApplyOnnxModel(
            modelFile: "ultraface.onnx",
            outputColumnNames: new[] { "scores", "boxes" },
            inputColumnNames: new[] { "input" });

        IDataView data = mlContext.Data.LoadFromEnumerable(new List<ModelInput>());

        var model = pipeline.Fit(data);

        predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
    }

    private void MlContext_Log(object? sender, LoggingEventArgs e)
    {
    }

    public List<Face> Process(SKBitmap bitmap)
    {
        var input = new ModelInput
        {
            Image = PreprocessImage(bitmap)
        };

        var result = predictionEngine.Predict(input);
        var faces = PostProcess(result.Scores, result.Boxes);

        return faces;
    }

    private static float[] PreprocessImage(SKBitmap bitmap)
    {;
        var resized = bitmap.Resize(new SKSizeI(Width, Height), SKFilterQuality.High);

        float[] imageData = new float[1 * 3 * Height * Width];

        int index = 0;
        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                var pixel = resized.GetPixel(x, y);

                imageData[index] = (pixel.Red - 127) / 128f;
                imageData[index + Height * Width] = (pixel.Green - 127) / 128f;
                imageData[index + 2 * Height * Width] = (pixel.Blue - 127) / 128f;

                index++;
            }
        }
        return imageData;
    }

    private static List<Face> PostProcess(float[] scores, float[] boxes, float confidenceThreshold = 0.5f, float iouThreshold = 0.5f)
    {
        var faces = new List<Face>();
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

    private static List<Face> NMS(List<Face> faces, float iouThreshold)
    {
        var sortedFaces = faces.OrderByDescending(f => f.Confidence).ToList();
        var selectedFaces = new List<Face>();

        while (sortedFaces.Any())
        {
            var bestFace = sortedFaces.First();
            selectedFaces.Add(bestFace);
            sortedFaces.RemoveAt(0);

            sortedFaces.RemoveAll(f => IOU(bestFace, f) > iouThreshold);
        }

        return selectedFaces;
    }

    private static float IOU(Face box1, Face box2)
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

public record Face(int X1, int Y1, int X2, int Y2, float Confidence)
{
    public override string ToString()
    {
        return $"{X1}:{Y1} {X2}:{Y2}; {Confidence}";
    }
}

public class ModelInput
{
    [VectorType(1, 3, 480, 640)]
    [ColumnName("input")]
    public float[] Image { get; set; }
}

public class ModelOutput
{
    [VectorType(1, 17640, 2)]
    [ColumnName("scores")]
    public float[] Scores { get; set; }

    [VectorType(1, 17640, 4)]
    [ColumnName("boxes")]
    public float[] Boxes { get; set; }
}
