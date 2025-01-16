namespace Deface.NET.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
        {
            options.FFMpegPath = "ffmpeg";
            options.FFProbePath = "ffprobe";
        });

        var result = defaceService.ProcessVideo(TestResources.TestResources.Video_Very_Short_480p, "test");
    }
}