using Deface.NET.Graphics;
using Deface.NET.VideoIO;

namespace Deface.NET.UnitTests;

public class VideoReaderUnitTests
{
    int i = 0;

    [Fact]
    public async Task Test()
    {
        var video = TestResources.TestResources.Video_Short_HD_1280_720_24fps;

        using VideoReader videoReader = new(video, null!, Process);
        await videoReader.Start();
    }

    private async Task Process(Frame bitmap, int index, int totalFrames)
    {
        i++;
    }
}
