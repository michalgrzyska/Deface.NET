﻿using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using Deface.NET.ObjectDetection.UltraFace;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;
using NSubstitute;

namespace Deface.NET.UnitTests.ObjectDetection;

[Collection(nameof(SettingsCollection))]
public class ObjectDetectorUnitTests
{
    private readonly SettingsFixture _settingsFixture;

    private readonly IUltraFaceDetector _faceDetector;
    private readonly ObjectDetector _objectDetector;

    private readonly List<DetectedObject> testData =
    [
        new(1, 1, 2, 2, 1),
        new(1, 1, 2, 2, 1),
        new(1, 1, 2, 2, 1),
        new(1, 1, 2, 2, 1),
    ];

    public ObjectDetectorUnitTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;

        _faceDetector = Substitute.For<IUltraFaceDetector>();
        _objectDetector = new(_faceDetector);

        _faceDetector
            .Detect(Arg.Any<Frame>(), Arg.Any<float>())
            .Returns(testData);
    }

    [Fact]
    public void Detect_DependenciesCalledCorrectlyAndObjectsAreResized()
    {
        // Arrange

        var settings = _settingsFixture.Settings;
        var frame = TestFrameHelper.GetTestFrame();

        // Act

        var objects = _objectDetector.Detect(frame, settings);

        // Assert

        _faceDetector.Received(1).Detect(Arg.Any<Frame>(), Arg.Any<float>());
        objects.Should().OnlyContain(x => x.IsResized);
    }
}
