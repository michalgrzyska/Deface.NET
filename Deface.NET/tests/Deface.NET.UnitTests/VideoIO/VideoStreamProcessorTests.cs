using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.UnitTests.VideoIO;

public class VideoStreamProcessorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    public void Process_NFrames_CorrectAmountOfProcessedFrames(int framesCount)
    {
        // Arrange 

        VideoInfo videoInfo = new(100, 100, 5, 20, 20, "");
        VideoStreamProcessor processor = new(videoInfo);

        using var stream = GetFramesStream(videoInfo, framesCount);

        // Act

        var frames = processor.ReadAllFrames(stream);

        // Assert

        frames.Count.Should().Be(framesCount);
    }

    private static MemoryStream GetFramesStream(VideoInfo videoInfo, int count = 1)
    {
        var frameSize = videoInfo.Width * videoInfo.Height * Frame.ChannelsCount;
        var result = new byte[frameSize * count];

        for (int i = 0; i < frameSize * count; i += Frame.ChannelsCount)
        {
            result[i] = 255;
            result[i + 1] = 0;
            result[i + 2] = 0;
            result[i + 3] = 255;
        }

        return new MemoryStream(result);
    }
}
