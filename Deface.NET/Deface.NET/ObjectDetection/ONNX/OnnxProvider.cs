using Microsoft.ML.OnnxRuntime;

namespace Deface.NET.ObjectDetection.ONNX;

internal class OnnxProvider : IOnnxProvider
{
    private const string CudaProvider = "CUDAExecutionProvider";

    private readonly string[] _providers;

    public OnnxProvider()
    {
         _providers = OrtEnv.Instance().GetAvailableProviders();
    }

    public bool IsGpuAvailable()
    {
        return _providers.Contains(CudaProvider);
    }
}
