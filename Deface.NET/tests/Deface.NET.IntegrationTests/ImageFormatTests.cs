using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class ImageFormatTests
{
    [Fact]
    public void ProcessImage_SettingPngFormat_ShouldSaveFileAsPng()
    {
        var deface = DefaceProvider.GetDefaceService(options =>
        {
            options.ImageFormat = ImageFormat.Png;
        });

        var result = deface.ProcessImage(TestResources.TestResources.Photo1, "output.png");

        Path.GetExtension(result.OutputFile).ShouldBe(".png");

        File.Delete(result.OutputFile);
    }

    [Fact]
    public void ProcessImage_SettingJpegFormat_ShouldSaveFileAsJpeg()
    {
        var deface = DefaceProvider.GetDefaceService(options =>
        {
            options.ImageFormat = ImageFormat.Jpeg();
        });

        var result = deface.ProcessImage(TestResources.TestResources.Photo1, "output.jpg");

        Path.GetExtension(result.OutputFile).ShouldBe(".jpg");

        File.Delete(result.OutputFile);
    }

    [Fact]
    public void ProcessImage_SettingPngFormat_TempPngFileShouldExist()
    {
        var deface = DefaceProvider.GetDefaceService(options =>
        {
            options.ImageFormat = ImageFormat.Png;
        });

        var result = deface.ProcessImage(TestResources.TestResources.Photo2, "output.png");

        File.Exists(result.OutputFile).ShouldBeTrue();

        File.Delete(result.OutputFile);
    }

    [Fact]
    public void ProcessImage_SettingPngFormat_TempJpegFileShouldExist()
    {
        var deface = DefaceProvider.GetDefaceService(options =>
        {
            options.ImageFormat = ImageFormat.Jpeg();
        });

        var result = deface.ProcessImage(TestResources.TestResources.Photo2, "output.jpg");

        File.Exists(result.OutputFile).ShouldBeTrue();

        File.Delete(result.OutputFile);
    }

    [Theory]
    [InlineData(65)]
    [InlineData(50)]
    public void ProcessImage_SettingJpgFormat_FileShouldBeCompressed(int imageQuality)
    {
        var deface = DefaceProvider.GetDefaceService(options =>
        {
            options.ImageFormat = ImageFormat.Jpeg(imageQuality);
        });

        var inputFileDir = TestResources.TestResources.Photo4;

        var result = deface.ProcessImage(inputFileDir, "output.jpg");

        FileInfo inputFileInfo = new(inputFileDir);
        var inputFileSize = inputFileInfo.Length;

        FileInfo outputFileInfo = new(result.OutputFile);
        var outputFileSize = outputFileInfo.Length;

        outputFileSize.ShouldBeLessThanOrEqualTo(inputFileSize);

        File.Delete(result.OutputFile);
    }
}
