using Deface.NET.Common;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.ONNX;
using Microsoft.ML;
using SkiaSharp;

namespace Deface.NET.ObjectDetection.UltraFace;

internal class UltraFaceDetector : OnnxDetectorBase<Input, Output>, IUltraFaceDetector
{
    private readonly PredictionEngine<Input, Output> _predictionEngine;

    private const int Width = 640;
    private const int Height = 480;
    private const float IouThreshold = 0.5f;

    // GpuDeviceId goes from global settings
    public UltraFaceDetector(IOnnxProvider onnxProvider, ISettingsProvider settingsProvider, IAppFiles appFiles)
        : base(onnxProvider, settingsProvider.Settings, appFiles.UltraFaceONNX)
    {
        _predictionEngine = GetPredictionEngine();
    }

    public List<DetectedObject> Detect(Frame frame, Settings settings)
    {
        var preprocessedImage = PreprocessImage(frame);
        Input input = new(preprocessedImage);

        var output = _predictionEngine.Predict(input);
        var result = PostProcess(output.Scores, output.Boxes, frame.Width, frame.Height, settings.FaceThreshold);

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
        var imageData = new float[1 * 3 * Height * Width];

        unsafe
        {
            var pixels = (byte*)resized.GetPixels();

            var totalPixels = Height * Width;
            var offsetR = 0;
            var offsetG = totalPixels;
            var offsetB = 2 * totalPixels;

            for (int i = 0; i < totalPixels; i++)
            {
                byte* pixel = pixels + i * 4;

                imageData[offsetR + i] = (pixel[0] - 127) / 128f;
                imageData[offsetG + i] = (pixel[1] - 127) / 128f;
                imageData[offsetB + i] = (pixel[2] - 127) / 128f;
            }
        }

        return imageData;
    }

    private static List<DetectedObject> PostProcess(float[] scores, float[] boxes, int originalW, int originalH, float confidenceThreshold)
    {
        List<DetectedObject> faces = [];
        var numBoxes = scores.Length / 2;

        for (int i = 0; i < numBoxes; i++)
        {
            var score = scores[i * 2 + 1];

            if (score > confidenceThreshold)
            {
                var x1 = boxes[i * 4] * Width;
                var y1 = boxes[i * 4 + 1] * Height;
                var x2 = boxes[i * 4 + 2] * Width;
                var y2 = boxes[i * 4 + 3] * Height;

                faces.Add(new((int)x1, (int)y1, (int)x2, (int)y2, score));
            }
        }

        var boxesNms = PostProcessingHelper.NMS(faces, IouThreshold);
        var rescaledBoxes = PostProcessingHelper.RescaleBoundingBoxes(boxesNms, originalW, originalH, Width, Height);

        return rescaledBoxes;
    }
}