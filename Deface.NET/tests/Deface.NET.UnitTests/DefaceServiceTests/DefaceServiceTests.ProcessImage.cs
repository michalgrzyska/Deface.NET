﻿using NSubstitute;

namespace Deface.NET.UnitTests.DefaceServiceTests;

public partial class DefaceServiceTests
{
    [Fact]
    public void ProcessImage_ProperData_DependenciesInvokedCorrectly()
    {
        // Arrange

        var input = Path.GetTempFileName();
        var output = "file";

        // Act

        _service.ProcessImage(input, output);

        // Assert

        _imageProcessor.Received(1).Process(Arg.Any<string>(), Arg.Any<string>());

        // Cleanup

        File.Delete(input);
    }

    [Fact]
    public void ProcessImage_InputNotExisting_ThrowsFileNotFoundException()
    {
        var action = () => _service.ProcessImage("test", "file");

        action.ShouldThrow<FileNotFoundException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessImage_InvalidInputString_ThrowsArgumentNullException(string? inputString)
    {
        var action = () => _service.ProcessImage(inputString!, "file");

        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ProcessImage_InvalidOutputString_ThrowsArgumentNullException(string? outputString)
    {
        // Arrange

        var input = Path.GetTempFileName();

        // Act

        var action = () => _service.ProcessImage(input, outputString!);

        // Assert

        action.ShouldThrow<ArgumentNullException>();

        // Cleanup

        File.Delete(input);
    }
}
