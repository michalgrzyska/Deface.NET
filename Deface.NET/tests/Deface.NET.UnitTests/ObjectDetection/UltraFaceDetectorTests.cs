﻿using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.ObjectDetection.ONNX;
using Deface.NET.ObjectDetection.UltraFace;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;
using NSubstitute;

namespace Deface.NET.UnitTests.ObjectDetection;

[Collection(nameof(SettingsCollection))]
public class UltraFaceDetectorTests : IDisposable
{
    private readonly UltraFaceDetector _detector;
    private readonly SettingsFixture _settingsFixture;

    public UltraFaceDetectorTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;

        var settingsProvider = Substitute.For<ISettingsProvider>();
        settingsProvider.Settings.Returns(_settingsFixture.Settings);

        _detector = new(GetOnnxProvider(), settingsProvider);
    }

    [Theory]
    [InlineData(TestResources.TestResources.Photo1, 1)]
    [InlineData(TestResources.TestResources.Photo2, 1)]
    [InlineData(TestResources.TestResources.Photo4, 7)]
    [InlineData(TestResources.TestResources.Photo6, 0)]
    public void Detect_ProperData_DetectsProperAmountOfFaces(string imagePath, int expectedFaces)
    {
        var settings = _settingsFixture.WithAction(x =>
        {
            x.Threshold = 0.8f;
        });
            
        var frame = TestFrameHelper.GetTestFrame(imagePath);

        var result = _detector.Detect(frame, settings);

        result.Count.ShouldBe(expectedFaces);
    }

    public void Dispose() => _detector.Dispose();

    private static IOnnxProvider GetOnnxProvider()
    {
        var provider = Substitute.For<IOnnxProvider>();

        provider.IsCpuAvailable().Returns(true);
        provider.IsGpuAvailable().Returns(true);

        return provider;
    }
}
