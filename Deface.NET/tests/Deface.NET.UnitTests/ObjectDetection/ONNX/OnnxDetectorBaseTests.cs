using Deface.NET.ObjectDetection.ONNX;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.ObjectDetection.ONNX;

[Collection(nameof(SettingsCollection))]
public class OnnxDetectorBaseTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void Ctor_GpuUnavailable_ThrowsDefaceException()
    {
        var onnxProvider = GetOnnxProvider(cpuAvailable: true, gpuAvailable: false);
        var settings = _settingsFixture.WithAction(x => x.Hardware = Hardware.Cuda(0));

        var action = () => new TestDetector(onnxProvider, settings, "");

        action.ShouldThrow<DefaceException>();
    }

    [Fact]
    public void Ctor_CpuUnavailable_ThrowsDefaceException()
    {
        var onnxProvider = GetOnnxProvider(cpuAvailable: false, gpuAvailable: false);
        var settings = _settingsFixture.WithAction(x => x.Hardware = Hardware.Cpu());

        var action = () => new TestDetector(onnxProvider, settings, "");

        action.ShouldThrow<DefaceException>();
    }

    private static IOnnxProvider GetOnnxProvider(bool cpuAvailable, bool gpuAvailable)
    {
        var provider = Substitute.For<IOnnxProvider>();

        provider.IsCpuAvailable().Returns(cpuAvailable);
        provider.IsGpuAvailable().Returns(gpuAvailable);

        return provider;
    }
}
