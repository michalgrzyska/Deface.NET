using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing;
using Deface.NET.System;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;
using NSubstitute;

namespace Deface.NET.UnitTests.Processing;

[Collection(nameof(SettingsCollection))]
public class ImageProcessorTests
{
    private readonly SettingsFixture _settingsFixture;

    private readonly IScopedSettingsProvider _settingsProvider = Substitute.For<IScopedSettingsProvider>();
    private readonly IDLogger<IDefaceService> _logger = Substitute.For<IDLogger<IDefaceService>>();
    private readonly IObjectDetector _detector = Substitute.For<IObjectDetector>();
    private readonly IShapeDrawerProvider _shapeDrawerProvider = Substitute.For<IShapeDrawerProvider>();
    private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();
    private readonly IFrameCreator _frameCreator = Substitute.For<IFrameCreator>();

    private readonly ImageProcessor _imageProcessor;

    public ImageProcessorTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;

        SetupMockMethods();

        _imageProcessor = new(_settingsProvider, _logger, _detector, _shapeDrawerProvider, _fileSystem, _frameCreator);
    }

    [Fact]
    public void Process_DependenciesCalledProperly()
    {
        // Arrange
        var shapeDrawer = Substitute.For<IShapeDrawer>();

        shapeDrawer
            .Draw(Arg.Any<Frame>(), Arg.Any<List<DetectedObject>>())
            .Returns(TestFrameHelper.GetTestFrame(TestResources.TestResources.Photo1));

        _shapeDrawerProvider.ShapeDrawer.Returns(shapeDrawer);


        // Act
        _imageProcessor.Process("input.mp4", "output.mp4");

        // Assert
        _detector.Received(1).Detect(Arg.Any<Frame>(), Arg.Any<Settings>());
        shapeDrawer.Received(1).Draw(Arg.Any<Frame>(), Arg.Any<List<DetectedObject>>());
        _fileSystem.Received(1).Save(Arg.Any<string>(), Arg.Any<byte[]>());
    }

    [Fact]
    public void ProcessMany_DependenciesCalledProperly()
    {
        // Arrange
        var shapeDrawer = Substitute.For<IShapeDrawer>();

        shapeDrawer
            .Draw(Arg.Any<Frame>(), Arg.Any<List<DetectedObject>>())
            .Returns((_) => TestFrameHelper.GetTestFrame(TestResources.TestResources.Photo1));

        _shapeDrawerProvider.ShapeDrawer.Returns(shapeDrawer);

        string[] systemFiles =
        [
            "file1.jpg",
            "file2.jpg",
            "file3.png",
            "file4.png",
            "file5.jpeg",
            "file6.exe"
        ];

        _fileSystem.GetFiles(Arg.Any<string>()).Returns(systemFiles);

        // Act

        _imageProcessor.ProcessMany("inputDir", "outputDir");

        // Assert

        _detector.Received(5).Detect(Arg.Any<Frame>(), Arg.Any<Settings>());
        shapeDrawer.Received(5).Draw(Arg.Any<Frame>(), Arg.Any<List<DetectedObject>>());
        _fileSystem.Received(5).Save(Arg.Any<string>(), Arg.Any<byte[]>());
    }

    private void SetupMockMethods()
    {
        _settingsProvider.Settings.Returns(_settingsFixture.Settings);

        _frameCreator
            .FromFile(Arg.Any<string>())
            .Returns((_) => TestFrameHelper.GetTestFrame(TestResources.TestResources.Photo1));

        _detector
            .Detect(Arg.Any<Frame>(), Arg.Any<Settings>())
            .Returns([]);
    }
}
