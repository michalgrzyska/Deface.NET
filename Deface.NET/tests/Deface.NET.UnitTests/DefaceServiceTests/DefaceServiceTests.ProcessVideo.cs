using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.DefaceServiceTests;

public partial class DefaceServiceTests
{
    [Fact]
    public void ProcessVideo_ProperData_DependenciesInvokedCorrectly()
    {
        // Arrange

        var input = Path.GetTempFileName();
        var output = "file";

        // Act

        _service.ProcessVideo(input, output);

        // Assert

        _videoProcessor.Received(1).Process(Arg.Any<string>(), Arg.Any<string>());

        // Cleanup

        File.Delete(input);
    }

    [Fact]
    public void ProcessVideo_InputNotExisting_ThrowsFileNotFoundException()
    {
        var action = () => _service.ProcessVideo("test", "file");
        action.ShouldThrow<DefaceException>().WithInnerException<FileNotFoundException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessVideo_InvalidInputString_ThrowsArgumenException(string? inputString)
    {
        var action = () => _service.ProcessVideo(inputString!, "file");
        action.ShouldThrow<DefaceException>().WithInnerException<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessVideo_InvalidOutputString_ThrowsArgumentException(string? outputString)
    {
        // Arrange

        var input = Path.GetTempFileName();

        // Act

        var action = () => _service.ProcessVideo(input, outputString!);

        // Assert

        action.ShouldThrow<DefaceException>().WithInnerException<ArgumentException>();

        // Cleanup

        File.Delete(input);
    }
}
