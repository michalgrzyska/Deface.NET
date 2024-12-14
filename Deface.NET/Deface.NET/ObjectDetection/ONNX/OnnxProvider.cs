using Microsoft.ML.OnnxRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.ONNX;

[ExcludeFromCodeCoverage]
internal class OnnxProvider : IOnnxProvider
{
    private const string CudaProvider = "CUDAExecutionProvider";
    private const string CpuProvider = "CPUExecutionProvider";

    private readonly string[] _providers;

    public OnnxProvider()
    {
         _providers = OrtEnv.Instance().GetAvailableProviders();
    }

    public bool IsGpuAvailable()
    {
        return _providers.Contains(CudaProvider);
    }

    public bool IsCpuAvailable() 
    {
        return _providers.Contains(CpuProvider); 
    }
}
