using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class FFMpegInvalidSourceFileTests
{
    [Fact]
    public void ProcessImage_NonExistingFile_ShouldThrowDefaceException()
    {
        var deface = DefaceProvider.GetDefaceService();

        var action = () => deface.ProcessImage("whatever", "output.jpg");

        action.ShouldThrow<DefaceException>().WithInnerException<FileNotFoundException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProcessImage_InvalidInputFileName_ShouldThrowDefaceException(string path)
    {
        var deface = DefaceProvider.GetDefaceService();

        var action = () => deface.ProcessImage(path, "output.jpg");

        action.ShouldThrow<DefaceException>().WithInnerException<ArgumentException>();
    }

    [Fact]
    public void ProcessImage_InvalidFile_ShouldThrowDefaceException()
    {
        var deface = DefaceProvider.GetDefaceService();

        var action = () => deface.ProcessImage(TestData.Images.CorruptedJPG, "output.jpg");

        action.ShouldThrow<DefaceException>().WithInnerException<InvalidOperationException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ProcessVideo_InvalidInputFileName_ShouldThrowDefaceException(string path)
    {
        var deface = DefaceProvider.GetDefaceService();

        var action = () => deface.ProcessVideo(path, "output.mp4");

        action.ShouldThrow<DefaceException>().WithInnerException<ArgumentException>();
    }

    [Fact]
    public void ProcessVideo_InvalidFile_ShouldThrowDefaceException()
    {
        var deface = DefaceProvider.GetDefaceService();

        var action = () => deface.ProcessVideo(TestData.Videos.CorruptedMP4, "output.mp4");

        action.ShouldThrow<DefaceException>().WithInnerException<InvalidOperationException>();
    }
}
