using Deface.NET.Configuration.MLHardware;
using Deface.NET.ObjectDetection.ONNX.Exceptions;
using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;

namespace Deface.NET.ObjectDetection.ONNX;

internal abstract class OnnxDetectorBase<TInput, TOutput> 
    where TInput : class 
    where TOutput : class
{
    protected readonly IOnnxProvider _onnxProvider;
    protected readonly MLContext _mlContext;
    protected readonly OnnxScoringEstimator _pipeline;

    protected readonly string[] _inputColumnNames;
    protected readonly string[] _outputColumnNames;
    protected readonly string _modelFile;

    public OnnxDetectorBase(IOnnxProvider onnxProvider, Settings settings, string modelFile)
    {
        _inputColumnNames = ColumnNameHelper.GetColumnNamesFrom<TInput>();
        _outputColumnNames = ColumnNameHelper.GetColumnNamesFrom<TOutput>();
        _modelFile = modelFile;

        _onnxProvider = onnxProvider;
        _mlContext = new();
        _pipeline = GetScoringEstimator(settings);
    }

    protected OnnxScoringEstimator GetScoringEstimator(Settings settings)
    {
        try
        {
            var estimator = settings.Hardware.Type switch
            {
                HardwareType.CUDA => GetCudaEstimator(settings.Hardware.GpuDeviceId!.Value),
                HardwareType.CPU => GetCpuEstimator(),

                _ => throw new NotImplementedException(),
            };

            return estimator;
        }
        catch (Exception e) 
        when (e is GpuUnavailableException || e is GpuIdNotFoundException)
        {
            throw new DefaceException(e.Message);
        }
    }

    private OnnxScoringEstimator GetCudaEstimator(int gpuId)
    {
        if (!_onnxProvider.IsGpuAvailable())
        {
            throw new GpuUnavailableException();
        }

        try
        {
            var estimator = _mlContext.Transforms.ApplyOnnxModel(
                modelFile: _modelFile,
                inputColumnNames: _inputColumnNames,
                outputColumnNames: _outputColumnNames,   
                gpuDeviceId: gpuId
            );

            return estimator;
        }
        catch (InvalidOperationException)
        {
            throw new GpuIdNotFoundException(gpuId);
        }
    }

    private OnnxScoringEstimator GetCpuEstimator()
    {
        var estimator = _mlContext.Transforms.ApplyOnnxModel(
            modelFile: _modelFile,
            inputColumnNames: _inputColumnNames,
            outputColumnNames: _outputColumnNames
        );

        return estimator;
    }
}
