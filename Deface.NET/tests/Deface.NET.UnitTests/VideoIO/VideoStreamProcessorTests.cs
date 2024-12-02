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

        List<FrameInfo> framesProcessed = [];
        Action<FrameInfo> action = framesProcessed.Add;

        // Act

        processor.Process(stream, action);

        // Assert

        framesProcessed.Count.Should().Be(framesCount);

        if (framesProcessed.Count > 0)
        {
            framesProcessed.Should().OnlyContain(x =>
                x.Width == videoInfo.Width &&
                x.Height == videoInfo.Height &&
                x.TotalFrames == videoInfo.TotalFrames);
        }
    }

    private static MemoryStream GetFramesStream(VideoInfo videoInfo, int count = 1)
    {
        var frameSize = videoInfo.Width * videoInfo.Height * 3;
        var result = new byte[frameSize * count];

        for (int i = 0; i < frameSize * count; i += 3)
        {
            result[i] = 255;
            result[i + 1] = 0;
            result[i + 2] = 0;
        }

        return new MemoryStream(result);
    }
}
