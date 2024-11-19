using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing;
using Deface.NET.System;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.Processing;

[Collection(nameof(SettingsCollection))]
public class ImageProcessorUnitTests
{
    private readonly SettingsFixture _settingsFixture;

    private readonly IScopedSettingsProvider _settingsProvider;
    private readonly IDLogger<IDefaceService> _logger;
    private readonly ObjectDetector _detector;
    private readonly IShapeDrawer _shapeDrawer;
    private readonly FileSystem _fileSystem;
    private readonly IFrameCreator _frameCreator;

    private readonly ImageProcessor _imageProcessor;

    public ImageProcessorUnitTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;

        _settingsProvider = Substitute.For<IScopedSettingsProvider>();
        _logger = Substitute.For<IDLogger<IDefaceService>>();
        _detector = Substitute.For<ObjectDetector>();
        _shapeDrawer = Substitute.For<IShapeDrawer>();
        _fileSystem = Substitute.For<FileSystem>();
        _frameCreator = Substitute.For<IFrameCreator>();

        _imageProcessor = new(_settingsProvider, _logger, _detector, _shapeDrawer, _fileSystem, _frameCreator);
    }

    [Fact]
    public void Process_DependenciesCalledProperly()
    {
        // Arrange

        _frameCreator
            .FromFile(Arg.Any<string>())
            .Returns(Arg.Any<Frame>());

        _detector
            .Detect(Arg.Any<Frame>(), Arg.Any<Settings>())
            .Returns([]);

        // Act

        _imageProcessor.Process("input.mp4", "output.mp4");

        // Assert

        _detector.Received(1).Detect(Arg.Any<Frame>(), Arg.Any<Settings>());
        _shapeDrawer.Received(1).DrawShapes(Arg.Any<Frame>(), Arg.Any<List<DetectedObject>>());
        _fileSystem.Received(1).Save(Arg.Any<string>(), Arg.Any<byte[]>());
    }
}
