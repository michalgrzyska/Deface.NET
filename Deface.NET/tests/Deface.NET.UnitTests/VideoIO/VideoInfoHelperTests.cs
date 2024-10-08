using Deface.NET.UnitTests.Graphics.Helpers;
using Deface.NET.VideoIO.Helpers;
using Deface.NET.VideoIO.Models;
using NSubstitute.ExceptionExtensions;

namespace Deface.NET.UnitTests.VideoIO;
public class VideoInfoHelperTests
{
    [Fact]
    public void ConvertOutputToVideoInfo_ShouldReturnValidVideoInfo_WhenValidOutputProvided()
    {
        // Arrange

        VideoInfoOutput output = new()
        {
            Streams =
            [
                new()
                {
                    Width = 1920,
                    Height = 1080,
                    Frames = "300",
                    TargetFrameRate = "30/1",
                    AverageFrameRate = "29/1"
                }
            ]
        };
        var filePath = "test.mp4";

        // Act

        var result = VideoInfoHelper.ConvertOutputToVideoInfo(output, filePath);

        // Assert

        result.ShouldNotBeNull();
        result.ShouldBe(1920, 1080);
        result.TotalFrames.ShouldBe(300);
        result.TargetFps.ShouldBe(30f);
        result.AverageFps.ShouldBe(29f);
        result.Path.ShouldBe(filePath);
    }

    [Fact]
    public void ConvertOutputToVideoInfo_ShouldThrowInvalidOperationException_WhenNoStreamsArePresent()
    {
        // Arrange

        VideoInfoOutput output = new() { Streams = [] };
        var filePath = "test.mp4";

        // Act

        var action = () => VideoInfoHelper.ConvertOutputToVideoInfo(output, filePath);

        // Assert

        action.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void ParseFrameRateString_ShouldReturnCorrectValue_WhenValidFractionProvided()
    {
        var frameRateString = "24/1";

        var result = VideoInfoHelper.ParseFrameRateString(frameRateString);

        result.ShouldBe(24f);
    }

    [Fact]
    public void ParseFrameRateString_ShouldThrowFormatException_WhenInvalidStringProvided()
    {
        var frameRateString = "invalid";

        var action = () => VideoInfoHelper.ParseFrameRateString(frameRateString);

        Should.Throw<FormatException>(() => action());
    }
}