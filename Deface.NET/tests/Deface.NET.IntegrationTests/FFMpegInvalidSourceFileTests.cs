using Deface.NET.Common;
using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class FFMpegInvalidSourceFileTests : BaseIntegrationTest
{
    [Fact]
    public void ProcessImage_NonExistingFile_ShouldThrowDefaceException()
    {
        var action = () => DefaceService().ProcessImage("whatever", "output.jpg");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<FileNotFoundException>(string.Format(ExceptionMessages.FileMustExist, "inputPath"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProcessImage_InvalidInputFileName_ShouldThrowDefaceException(string path)
    {
        var action = () => DefaceService().ProcessImage(path, "output.jpg");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<ArgumentException>(string.Format(ExceptionMessages.MustNotBeNullOrWhiteSpace, "inputPath"));
    }

    [Fact]
    public void ProcessImage_InvalidFile_ShouldThrowDefaceException()
    {
        var action = () => DefaceService().ProcessImage(TestData.Images.CorruptedJPG, "output.jpg");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<InvalidOperationException>(string.Format(ExceptionMessages.FailedToDecodeImage, TestData.Images.CorruptedJPG));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProcessVideo_InvalidInputFileName_ShouldThrowDefaceException(string path)
    {
        var action = () => DefaceService().ProcessVideo(path, "output.mp4");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<ArgumentException>(string.Format(ExceptionMessages.MustNotBeNullOrWhiteSpace, "inputPath"));
    }

    [Fact]
    public void ProcessVideo_InvalidFile_ShouldThrowDefaceException()
    {
        var action = () => DefaceService().ProcessVideo(TestData.Videos.CorruptedMP4, "output.mp4");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<InvalidOperationException>(ExceptionMessages.VideoHasNoStreams);
    }
}
