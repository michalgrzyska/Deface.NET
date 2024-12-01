using Deface.NET.VideoIO.Helpers;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.UnitTests.VideoIO;
public class VideoInfoHelperTests
{
    [Fact]
    public void ConvertOutputToVideoInfo_ShouldReturnValidVideoInfo_WhenValidOutputProvided()
    {
        // Arrange
        var output = new VideoInfoOutput
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
        string filePath = "test.mp4";

        // Act
        var result = VideoInfoHelper.ConvertOutputToVideoInfo(output, filePath);

        // Assert
        result.Should().NotBeNull();
        result.Width.Should().Be(1920);
        result.Height.Should().Be(1080);
        result.TotalFrames.Should().Be(300);
        result.TargetFps.Should().Be(30f);
        result.AverageFps.Should().Be(29f);
        result.Path.Should().Be(filePath);
    }

    [Fact]
    public void ConvertOutputToVideoInfo_ShouldThrowInvalidOperationException_WhenNoStreamsArePresent()
    {
        // Arrange
        var output = new VideoInfoOutput
        {
            Streams = []
        };
        string filePath = "test.mp4";

        // Act
        Action action = () => VideoInfoHelper.ConvertOutputToVideoInfo(output, filePath);

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ParseFrameRateString_ShouldReturnCorrectValue_WhenValidFractionProvided()
    {
        // Arrange
        string frameRateString = "24/1";

        // Act
        var result = VideoInfoHelper.ParseFrameRateString(frameRateString);

        // Assert
        result.Should().Be(24f);
    }

    [Fact]
    public void ParseFrameRateString_ShouldThrowFormatException_WhenInvalidStringProvided()
    {
        // Arrange
        string frameRateString = "invalid";

        // Act
        Action action = () => VideoInfoHelper.ParseFrameRateString(frameRateString);

        // Assert
        action.Should().Throw<FormatException>();
    }
}