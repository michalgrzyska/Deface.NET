using Deface.NET.ObjectDetection.ONNX;

namespace Deface.NET.UnitTests.ObjectDetection.ONNX;

internal class TestDetector(IOnnxProvider onnxProvider, Settings settings, string modelFile)
    : OnnxDetectorBase<TestInput, TestOutput>(onnxProvider, settings, modelFile);
