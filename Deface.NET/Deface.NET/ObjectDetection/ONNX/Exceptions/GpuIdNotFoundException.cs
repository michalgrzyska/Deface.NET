namespace Deface.NET.ObjectDetection.ONNX.Exceptions;

internal class GpuIdNotFoundException(int gpuId) : Exception($"GPU with ID {gpuId} is not found.");