using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.ONNX.Exceptions;

[ExcludeFromCodeCoverage]
internal class GpuIdNotFoundException(int gpuId) : Exception($"GPU with ID {gpuId} is not found.");