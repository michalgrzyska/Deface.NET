using Deface.NET.Common;
using Deface.NET.System;
using NSubstitute;

namespace Deface.NET.UnitTests.Common;

public class AppFilesTests
{
    private const string BaseDirectory = "/app";
    private const string FileName = "ultraface.onnx";
    private readonly string FullPath = Path.Combine(BaseDirectory, "Resources", FileName);

    [Fact]
    public void UltraFaceONNX_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var fileSystem = Substitute.For<IFileSystem>();

        fileSystem.BaseDirectory.Returns(BaseDirectory);
        fileSystem.Exists(Arg.Any<string>()).Returns(false);

        AppFiles appFiles = new(fileSystem);

        // Act & Assert
        var action = () => _ = appFiles.UltraFaceONNX;
        action.ShouldThrow<FileNotFoundException>(string.Format(ExceptionMessages.AppFileNotFound, FileName));
    }

    [Fact]
    public void UltraFaceONNX_ShouldReturnFullPathWhenFileExists()
    {
        // Arrange
        var fileSystem = Substitute.For<IFileSystem>();

        fileSystem.BaseDirectory.Returns(BaseDirectory);
        fileSystem.Exists(Arg.Any<string>()).Returns(true);

        AppFiles appFiles = new(fileSystem);

        // Act
        var result = appFiles.UltraFaceONNX;

        // Assert
        result.ShouldBe(FullPath);
    }
}
