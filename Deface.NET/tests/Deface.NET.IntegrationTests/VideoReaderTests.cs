using Deface.NET.IntegrationTests.Helpers.VideoReader;

namespace Deface.NET.IntegrationTests;

public class VideoReaderTests
{
    [Fact]
    public void ReadingVideo_ShouldReadCorrectBitmaps()
    {
        VideoReaderProvider videoReader = new();

        var result = videoReader.ReadVideo(TestResources.TestResources.Video_Short_640_360_24fps);

        result.Frames.Count.ShouldBeGreaterThan(0);
        result.VideoInfo.Width.ShouldBeGreaterThan(0);
        result.VideoInfo.Height.ShouldBeGreaterThan(0);

        result.Frames.First().Width.ShouldBe(result.VideoInfo.Width);
        result.Frames.First().Height.ShouldBe(result.VideoInfo.Height);
    }
}
