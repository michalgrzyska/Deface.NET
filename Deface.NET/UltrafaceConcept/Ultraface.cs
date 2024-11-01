using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.Transforms.Onnx;
using System.Drawing;

namespace UltrafaceConcept;

internal class Ultraface
{
    private MLContext mlContext;
    private OnnxScoringEstimator pipeline;
    private PredictionEngine<ModelInput, ModelOutput> predictionEngine;

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

    public List<Face> Process(string imagePath)
    {
        var input = new ModelInput
        {
            Image = PreprocessImage(imagePath)
        };

        var result = predictionEngine.Predict(input);
        var faces = PostProcess(result.Scores, result.Boxes);

        return faces;
    }

    private static float[] PreprocessImage(string imagePath)
    {
        using Bitmap bitmap = new Bitmap(imagePath);

        // Convert to RGB and resize to 640x480
        using Bitmap resized = new Bitmap(bitmap, new Size(640, 480));
        float[] imageData = new float[1 * 3 * 480 * 640];

        int index = 0;
        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                Color pixel = resized.GetPixel(x, y);

                // Subtract mean and scale
                imageData[index] = (pixel.R - 127) / 128f; // Red channel
                imageData[index + 480 * 640] = (pixel.G - 127) / 128f; // Green channel
                imageData[index + 2 * 480 * 640] = (pixel.B - 127) / 128f; // Blue channel

                index++;
            }
        }
        return imageData;
    }

    private static List<Face> PostProcess(float[] scores, float[] boxes, float confidenceThreshold = 0.5f, float iouThreshold = 0.5f)
    {
        var faces = new List<Face>();
        int numBoxes = scores.Length / 2; // There are two scores for each box

        for (int i = 0; i < numBoxes; i++)
        {
            float score = scores[i * 2 + 1]; // Confidence for face presence

            if (score > confidenceThreshold)
            {
                // Extract bounding box coordinates
                float x1 = boxes[i * 4] * 640;
                float y1 = boxes[i * 4 + 1] * 480;
                float x2 = boxes[i * 4 + 2] * 640;
                float y2 = boxes[i * 4 + 3] * 480;

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
