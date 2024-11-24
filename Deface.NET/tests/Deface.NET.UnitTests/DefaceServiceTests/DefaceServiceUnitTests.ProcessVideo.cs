using NSubstitute;

namespace Deface.NET.UnitTests.DefaceServiceTests;

public partial class DefaceServiceUnitTests
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

        action.Should().Throw<FileNotFoundException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessVideo_InvalidInputString_ThrowsArgumentNullException(string? inputString)
    {
        var action = () => _service.ProcessVideo(inputString!, "file");

        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessVideo_InvalidOutputString_ThrowsArgumentNullException(string? outputString)
    {
        // Arrange

        var input = Path.GetTempFileName();

        // Act

        var action = () => _service.ProcessVideo(input, outputString);

        // Assert

        action.Should().Throw<ArgumentNullException>();

        // Cleanup

        File.Delete(input);
    }
}
