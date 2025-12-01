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
    private const float IouThreshold = 0.45f;
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

        detections = PostProcessingHelper.NMS(detections, IouThreshold);
        return PostProcessingHelper.RescaleBoundingBoxes(detections, originalW, originalH, Width, Height);
    }
}