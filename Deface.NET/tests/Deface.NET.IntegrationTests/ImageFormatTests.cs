using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class ImageFormatTests : IOTest
{
    [Fact]
    public void ProcessImage_SettingPngFormat_ShouldSaveFileAsPng()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceProvider.GetDefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Png;
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo1, "output.png");

            Path.GetExtension(result.OutputFile).ShouldBe(".png");
        }
        finally
        {
            Cleanup(result?.OutputFile);
        }
    }

    [Fact]
    public void ProcessImage_SettingJpegFormat_ShouldSaveFileAsJpeg()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceProvider.GetDefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Jpeg();
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo1, "output.jpg");

            Path.GetExtension(result.OutputFile).ShouldBe(".jpg");
        }
        finally
        {
            Cleanup(result?.OutputFile);
        }
    }

    [Fact]
    public void ProcessImage_SettingPngFormat_TempPngFileShouldExist()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceProvider.GetDefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Png;
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo2, "output.png");

            File.Exists(result.OutputFile).ShouldBeTrue();
        }
        finally
        {
            Cleanup(result?.OutputFile);
        }
    }

    [Fact]
    public void ProcessImage_SettingPngFormat_TempJpegFileShouldExist()
    {
        ProcessingResult result = default;

        try
        {
            var deface = DefaceProvider.GetDefaceService(options =>
            {
                options.ImageFormat = ImageFormat.Jpeg();
            });

            result = deface.ProcessImage(TestResources.TestResources.Photo2, "output.jpg");

            File.Exists(result.OutputFile).ShouldBeTrue();
        }
        finally
        {
            Cleanup(result?.OutputFile);
        }
    }
}
