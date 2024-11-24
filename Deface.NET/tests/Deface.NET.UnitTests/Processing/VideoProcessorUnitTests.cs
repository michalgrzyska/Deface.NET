using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Deface.NET.UnitTests.Processing;

[Collection(nameof(SettingsCollection))]
public class VideoProcessorUnitTests
{
    private readonly SettingsFixture _settingsFixture;

    private readonly IDLogger<IDefaceService> _logger = Substitute.For<IDLogger<IDefaceService>>();
    private readonly IObjectDetector _detector = Substitute.For<IObjectDetector>();
    private readonly IShapeDrawerProvider _shapeDrawerProvider = Substitute.For<IShapeDrawerProvider>();
    private readonly IVideoReader _videoReader = Substitute.For<IVideoReader>();
    private readonly IVideoWriter _videoWriter = Substitute.For<IVideoWriter>();
    private readonly IFrameCreator _frameCreator = Substitute.For<IFrameCreator>();

    private const int FrameSize = 1000;
    private const int Fps = 10;

    public VideoProcessorUnitTests(SettingsFixture settingsFixture)
    {
        _settingsFixture = settingsFixture;
        SetupMockMethods();
    }

    [Fact]
    public async Task Process_DependenciesCalledProperly()
    {
        // Arrange 

        var processor = GetVideoProcessor();
        int framesCount = SetupVideoReader();

        var shapeDrawer = Substitute.For<IShapeDrawer>();
        _shapeDrawerProvider.ShapeDrawer.Returns(shapeDrawer);

        // Act

        var result = await processor.Process("input", "output");

        // Assert

        _detector.Received(framesCount).Detect(Arg.Any<Frame>(), Arg.Any<Settings>());
        shapeDrawer.Received(framesCount).Draw(Arg.Any<Frame>(), Arg.Any<List<DetectedObject>>());
        _videoWriter.Received(1).WriteVideo(Arg.Any<List<Frame>>(), Arg.Any<VideoInfo>(), Arg.Any<string>());
        await _videoReader.Received(1).ReadVideo(Arg.Any<Func<FrameInfo, Task>>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Process_ResultAsExpected()
    {
        // Arrange 

        var processor = GetVideoProcessor();
        var inputFile = "input";
        var outputFile = "output";
        int framesCount = SetupVideoReader();

        // Act

        var result = await processor.Process(inputFile, outputFile);

        // Assert

        result.Fps.Should().Be(Fps);
        result.InputFile.Should().Be(inputFile);
        result.OutputFile.Should().Be(outputFile);
        result.ProcessingTime.Should().NotBe(TimeSpan.Zero);
        result.Threshold.Should().Be(_settingsFixture.Settings.Threshold);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Process_RunDetectionEachNFrames_DependenciesCalledProperly(int runDetectionEachNFrames)
    {
        // Arrange 

        int framesCount = SetupVideoReader();
        int timesDetectionShouldBeRan = (int)Math.Ceiling((double)(framesCount / (runDetectionEachNFrames + 0.0)));

        var processor = GetVideoProcessor(x =>
        {
            x.RunDetectionEachNFrames = runDetectionEachNFrames;
        });

        // Act

        var result = await processor.Process("input", "output");

        // Assert

        _detector.Received(timesDetectionShouldBeRan).Detect(Arg.Any<Frame>(), Arg.Any<Settings>());
    }

    private void SetupMockMethods()
    {
        _logger
            .GetProgressLogger()
            .Returns(new DProgressLogger<IDefaceService>(Substitute.For<ILogger<IDefaceService>>(), LoggingLevel.None));
    }

    private VideoProcessor GetVideoProcessor(Action<Settings>? action = null)
    {
        var settings = _settingsFixture.WithAction(action);

        IScopedSettingsProvider settingsProvider = Substitute.For<IScopedSettingsProvider>();
        settingsProvider.Settings.Returns(settings);

        return new(settingsProvider, _logger, _detector, _videoWriter, _videoReader, _shapeDrawerProvider, _frameCreator);
    }

    private int SetupVideoReader()
    {
        List<Frame> mockFrames = Enumerable
            .Range(1, 10)
            .Select(x => ProcessingTestHelper.GetTestFrame())
            .ToList();

        _videoReader
            .ReadVideo(Arg.Any<Func<FrameInfo, Task>>(), Arg.Any<string>())
            .Returns(async call =>
            {
                var frameCallback = call.Arg<Func<FrameInfo, Task>>();

                for (int i = 0; i < mockFrames.Count; i++)
                {
                    FrameInfo fi = new(mockFrames[i].ToByteArray(), i, mockFrames.Count, mockFrames[i].Width, mockFrames[i].Height);
                    await frameCallback(fi);
                }

                return new VideoInfo(FrameSize, FrameSize, mockFrames.Count, Fps, Fps);
            });

        return mockFrames.Count;
    }
}
