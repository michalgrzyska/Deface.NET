using Deface.NET.IntegrationTests.Helpers;
using Deface.NET.IntegrationTests.Helpers.VideoReading;
using SkiaSharp;
using static Deface.NET.IntegrationTests.Resources.TestData;

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

            testVideo.Frames.ElementAt(1).HasRectangle(103, 84, 271, 300, color);
            testVideo.Frames.ElementAt(33).HasRectangle(1039, 81, 1205, 303, color);
            testVideo.Frames.ElementAt(63).HasRectangle(1039, 457, 1203, 681, color);
            testVideo.Frames.ElementAt(95).HasRectangle(105, 461, 269, 683, color);
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }
}
