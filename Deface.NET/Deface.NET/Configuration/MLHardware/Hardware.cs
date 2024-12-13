using Deface.NET.Configuration.MLHardware;

namespace Deface.NET;

/// <summary>
/// Specifies the hardware to be used for object detection.
/// </summary>
public readonly struct Hardware
{
    internal HardwareType Type { get; private init; }
    internal int? GpuDeviceId { get; private init; }

    /// <summary>
    /// Use CPU for object detection.
    /// </summary>
    public static Hardware Cpu() => new(HardwareType.CPU);

    /// <summary>
    /// Use GPU with CUDA for object detection.
    /// </summary>
    /// <param name="gpuDeviceId">ID of a GPU. Most common are 0 and 1.</param>
    public static Hardware Cuda(int gpuDeviceId) => new(HardwareType.CUDA, gpuDeviceId);

    private Hardware(HardwareType type, int? gpuDeviceId = default)
    {
        Type = type;
        GpuDeviceId = gpuDeviceId;
    }
}
