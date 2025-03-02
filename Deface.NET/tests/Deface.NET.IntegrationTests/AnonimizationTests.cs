using Deface.NET.IntegrationTests.Helpers;
using Deface.NET.IntegrationTests.Helpers.VideoReading;
using Deface.NET.Tests.Shared.Helpers;
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

            ValidateRectangle(testVideo.Frames.ElementAt(1), 103, 84, 271, 300, color);
            ValidateRectangle(testVideo.Frames.ElementAt(33), 1039, 81, 1205, 303, color);
            ValidateRectangle(testVideo.Frames.ElementAt(63), 1039, 457, 1203, 681, color);
            ValidateRectangle(testVideo.Frames.ElementAt(95), 105, 461, 269, 683, color);
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }

    private void ValidateRectangle(TestFrame frame, int x1, int y1, int x2, int y2, SKColor color)
    {
        ShapeValidationHelper.ValidateRectangle(frame, x1, y1, x2, y2, pixel =>
        {
            var colorsMatch =
                pixel.R == color.Red &&
                pixel.G == color.Green &&
                pixel.B == color.Blue;

            colorsMatch.ShouldBeTrue();
        });
    }
}
