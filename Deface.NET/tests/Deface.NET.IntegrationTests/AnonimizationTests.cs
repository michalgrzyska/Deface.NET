using Deface.NET.IntegrationTests.Helpers;
using Deface.NET.IntegrationTests.Helpers.VideoReading;
using SkiaSharp;

namespace Deface.NET.IntegrationTests;

public class AnonimizationTests : BaseIntegrationTest
{
    [Fact]
    public async Task ProcessingVideo_UsingRectangleAnonimization_ShouldCreateRectangle()
    {
        ProcessingResult result = default;

        try
        {
            const string outputFileName = "output.mp4";

            const byte red = 0;
            const byte green = 0;
            const byte blue = 0;

            var deface = DefaceService(options =>
            {
                options.AnonimizationMethod = AnonimizationMethod.Color(red, green, blue);
                options.AnonimizationShape = AnonimizationShape.Rectangle;

                options.EncodingCodec = EncodingCodec.H264;
            });

            result = deface.ProcessVideo(TestResources.TestResources.Video_Kappa, outputFileName);

            var testVideo = await TestVideo.Get(outputFileName);

            SKColor color = new(red, green, blue);

            TestRectangle(testVideo, 103, 84, 271, 300, color, 0, 125);
            TestRectangle(testVideo, 1039, 81, 1205, 303, color, 32, 125);
            TestRectangle(testVideo, 1039, 457, 1203, 681, color, 62, 125);
            TestRectangle(testVideo, 105, 461, 269, 683, color, 94, 125);
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }

    private static void TestRectangle(TestVideo video, int x1, int y1, int x2, int y2, SKColor color, int startFrame, int endFrame)
    {
        for (int currentFrame = startFrame; currentFrame < endFrame; currentFrame++)
        {
            video.Frames.ElementAt(currentFrame).HasRectangle(x1, y1, x2, y2, color);
        }
    }
}
