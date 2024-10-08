using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.ONNX.Exceptions;

[ExcludeFromCodeCoverage]
internal class GpuUnavailableException() : Exception("CUDA provider is not available. Check if your GPU is compatible and if your versions of CUDA and cuDNN are compatible: https://docs.nvidia.com/deeplearning/cudnn/latest/reference/support-matrix.html.");
