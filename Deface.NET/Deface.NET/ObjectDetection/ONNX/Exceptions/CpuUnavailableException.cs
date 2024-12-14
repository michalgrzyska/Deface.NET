using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.ONNX.Exceptions;

[ExcludeFromCodeCoverage]
internal class CpuUnavailableException() : Exception("CPU provider is not available. Check you settings.");