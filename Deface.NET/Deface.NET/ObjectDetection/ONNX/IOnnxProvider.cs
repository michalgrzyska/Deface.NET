namespace Deface.NET.ObjectDetection.ONNX;

internal interface IOnnxProvider
{
    bool IsCpuAvailable();
    bool IsGpuAvailable();
}