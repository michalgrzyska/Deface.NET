using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class ImageFormatTests : BaseIntegrationTest
{
    [Fact]
    public void ProcessImage_SettingPngFormat_ShouldSaveFileAsPng()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Png;
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo1, "output.png");

            Path.GetExtension(result.OutputFile).ShouldBe(".png");
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }

    [Fact]
    public void ProcessImage_SettingJpegFormat_ShouldSaveFileAsJpeg()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Jpeg();
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo1, "output.jpg");

            Path.GetExtension(result.OutputFile).ShouldBe(".jpg");
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }

    [Fact]
    public void ProcessImage_SettingPngFormat_TempPngFileShouldExist()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Png;
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo2, "output.png");

            File.Exists(result.OutputFile).ShouldBeTrue();
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }

    [Fact]
    public void ProcessImage_SettingPngFormat_TempJpegFileShouldExist()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Jpeg();
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo2, "output.jpg");

            File.Exists(result.OutputFile).ShouldBeTrue();
        }
        finally
        {
            CleanupFiles(result?.OutputFile);
        }
    }
}
