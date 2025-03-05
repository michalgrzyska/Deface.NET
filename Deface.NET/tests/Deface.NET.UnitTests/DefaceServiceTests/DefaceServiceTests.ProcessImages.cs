using Deface.NET.Common;
using Deface.NET.UnitTests._TestsConfig;
using NSubstitute;

namespace Deface.NET.UnitTests.DefaceServiceTests;

public partial class DefaceServiceTests
{
    [Fact]
    public void ProcessImages_ProperData_DependenciesInvokedCorrectly()
    {
        // Arrange

        var input = GetTempDirectory();
        var output = GetTempDirectory();

        // Act

        _service.ProcessImages(input, output);

        // Assert

        _imageProcessor.Received(1).ProcessMany(Arg.Any<string>(), Arg.Any<string>());

        // Cleanup

        Directory.Delete(input);
        Directory.Delete(output);
    }

    [Fact]
    public void ProcessImages_InputNotExisting_ThrowsDirectoryNotFoundException()
    {
        var action = () => _service.ProcessImages("test", "file");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<DirectoryNotFoundException>(string.Format(ExceptionMessages.DirectoryMustExist, "inputDirectory"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessImages_InvalidInputString_ThrowsArgumentException(string? inputString)
    {
        var action = () => _service.ProcessImages(inputString!, "file");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<ArgumentException>(string.Format(ExceptionMessages.MustNotBeNullOrWhiteSpace, "inputDirectory"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessImages_InvalidOutputString_ThrowsArgumentException(string? outputString)
    {
        // Arrange

        var input = GetTempDirectory();

        // Act

        var action = () => _service.ProcessImages(input, outputString!);

        // Assert

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<ArgumentException>(string.Format(ExceptionMessages.MustNotBeNullOrWhiteSpace, "outputDirectory"));

        // Cleanup

        Directory.Delete(input);
    }

    private static string GetTempDirectory()
    {
        string tempDirectory = Path.GetTempPath();
        string newTempDirectory = Path.Combine(tempDirectory, Guid.NewGuid().ToString());

        Directory.CreateDirectory(newTempDirectory);

        return newTempDirectory;
    }
}
