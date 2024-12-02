using Deface.NET.Common;

namespace Deface.NET.UnitTests;

public class ValidationHelperTests
{
    [Theory]
    [InlineData(5, 3)]
    [InlineData(5, 5)]
    public void MustBeGreaterOrEqualTo_WithValidValues_ShouldNotThrow(int prop, int value)
    {
        var act = () => ValidationHelper.MustBeGreaterOrEqualTo(prop, value, nameof(prop));
        act.Should().NotThrow();
    }

    [Fact]
    public void MustBeGreaterOrEqualTo_WithInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        var prop = 2;
        var value = 5;

        var act = () => ValidationHelper.MustBeGreaterOrEqualTo(prop, value, nameof(prop));
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(5, 5)]
    public void MustBeLessThanOrEqualTo_WithValidValues_ShouldNotThrow(int prop, int value)
    {
        var act = () => ValidationHelper.MustBeLessThanOrEqualTo(prop, value, nameof(prop));
        act.Should().NotThrow();
    }

    [Fact]
    public void MustBeLessThanOrEqualTo_WithInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        var prop = 7;
        var value = 5;

        var act = () => ValidationHelper.MustBeLessThanOrEqualTo(prop, value, nameof(prop));
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("valid string")]
    [InlineData("also_valid")]
    public void MustNotBeNullOrWhiteSpace_WithValidStrings_ShouldNotThrow(string prop)
    {
        var act = () => ValidationHelper.MustNotBeNullOrWhiteSpace(prop, nameof(prop));
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MustNotBeNullOrWhiteSpace_WithInvalidStrings_ShouldThrowArgumentNullException(string? prop)
    {
        var act = () => ValidationHelper.MustNotBeNullOrWhiteSpace(prop!, nameof(prop));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FileMustExist_WithExistingFile_ShouldNotThrow()
    {
        var existingFilePath = Path.GetTempFileName();

        var act = () => ValidationHelper.FileMustExist(existingFilePath, nameof(existingFilePath));
        act.Should().NotThrow();

        File.Delete(existingFilePath);
    }

    [Fact]
    public void FileMustExist_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        var nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.FileMustExist(nonExistentFilePath, nameof(nonExistentFilePath));
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void DirectoryMustExist_WithExistingDirectory_ShouldNotThrow()
    {
        string existingDirectoryPath = Path.GetTempPath();

        var act = () => ValidationHelper.DirectoryMustExist(existingDirectoryPath, nameof(existingDirectoryPath));
        act.Should().NotThrow();
    }

    [Fact]
    public void DirectoryMustExist_WithNonExistentDirectory_ShouldThrowDirectoryNotFoundException()
    {
        var nonExistentDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.DirectoryMustExist(nonExistentDirectoryPath, nameof(nonExistentDirectoryPath));
        act.Should().Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void ValidateFilePath_WithValidFilePath_ShouldNotThrow()
    {
        string existingFilePath = Path.GetTempFileName();

        var act = () => ValidationHelper.ValidateFilePath(existingFilePath, nameof(existingFilePath));
        act.Should().NotThrow();

        File.Delete(existingFilePath);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateFilePath_WithInvalidPath_ShouldThrowArgumentNullException(string? path)
    {
        var act = () => ValidationHelper.ValidateFilePath(path!, nameof(path));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValidateFilePath_WithNonExistentFilePath_ShouldThrowFileNotFoundException()
    {
        var nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.ValidateFilePath(nonExistentFilePath, nameof(nonExistentFilePath));
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void ValidateDirectoryPath_WithValidDirectoryPath_ShouldNotThrow()
    {
        var existingDirectoryPath = Path.GetTempPath();

        var act = () => ValidationHelper.ValidateDirectoryPath(existingDirectoryPath, nameof(existingDirectoryPath));
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateDirectoryPath_WithInvalidPath_ShouldThrowArgumentNullException(string? path)
    {
        var act = () => ValidationHelper.ValidateDirectoryPath(path!, nameof(path));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValidateDirectoryPath_WithNonExistentDirectoryPath_ShouldThrowDirectoryNotFoundException()
    {
        string nonExistentDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.ValidateDirectoryPath(nonExistentDirectoryPath, nameof(nonExistentDirectoryPath));
        act.Should().Throw<DirectoryNotFoundException>();
    }
}
