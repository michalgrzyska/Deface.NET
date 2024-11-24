using Deface.NET.Common;

namespace Deface.NET.UnitTests;

public class ValidationHelperTests
{
    [Theory]
    [InlineData(5, 3)]
    [InlineData(5, 5)]
    public void MustBeGreaterOrEqualTo_WithValidValues_ShouldNotThrow(int prop, int value)
    {
        Action act = () => ValidationHelper.MustBeGreaterOrEqualTo(prop, value, nameof(prop));
        act.Should().NotThrow();
    }

    [Fact]
    public void MustBeGreaterOrEqualTo_WithInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        int prop = 2;
        int value = 5;

        Action act = () => ValidationHelper.MustBeGreaterOrEqualTo(prop, value, nameof(prop));
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(5, 5)]
    public void MustBeLessThanOrEqualTo_WithValidValues_ShouldNotThrow(int prop, int value)
    {
        Action act = () => ValidationHelper.MustBeLessThanOrEqualTo(prop, value, nameof(prop));
        act.Should().NotThrow();
    }

    [Fact]
    public void MustBeLessThanOrEqualTo_WithInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        int prop = 7;
        int value = 5;

        Action act = () => ValidationHelper.MustBeLessThanOrEqualTo(prop, value, nameof(prop));
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("valid string")]
    [InlineData("also_valid")]
    public void MustNotBeNullOrWhiteSpace_WithValidStrings_ShouldNotThrow(string prop)
    {
        Action act = () => ValidationHelper.MustNotBeNullOrWhiteSpace(prop, nameof(prop));
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MustNotBeNullOrWhiteSpace_WithInvalidStrings_ShouldThrowArgumentNullException(string prop)
    {
        Action act = () => ValidationHelper.MustNotBeNullOrWhiteSpace(prop, nameof(prop));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FileMustExist_WithExistingFile_ShouldNotThrow()
    {
        string existingFilePath = Path.GetTempFileName();

        Action act = () => ValidationHelper.FileMustExist(existingFilePath, nameof(existingFilePath));
        act.Should().NotThrow();

        File.Delete(existingFilePath);
    }

    [Fact]
    public void FileMustExist_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Action act = () => ValidationHelper.FileMustExist(nonExistentFilePath, nameof(nonExistentFilePath));
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void DirectoryMustExist_WithExistingDirectory_ShouldNotThrow()
    {
        string existingDirectoryPath = Path.GetTempPath();

        Action act = () => ValidationHelper.DirectoryMustExist(existingDirectoryPath, nameof(existingDirectoryPath));
        act.Should().NotThrow();
    }

    [Fact]
    public void DirectoryMustExist_WithNonExistentDirectory_ShouldThrowDirectoryNotFoundException()
    {
        string nonExistentDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Action act = () => ValidationHelper.DirectoryMustExist(nonExistentDirectoryPath, nameof(nonExistentDirectoryPath));
        act.Should().Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void ValidateFilePath_WithValidFilePath_ShouldNotThrow()
    {
        string existingFilePath = Path.GetTempFileName();

        Action act = () => ValidationHelper.ValidateFilePath(existingFilePath, nameof(existingFilePath));
        act.Should().NotThrow();

        File.Delete(existingFilePath);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateFilePath_WithInvalidPath_ShouldThrowArgumentNullException(string path)
    {
        Action act = () => ValidationHelper.ValidateFilePath(path, nameof(path));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValidateFilePath_WithNonExistentFilePath_ShouldThrowFileNotFoundException()
    {
        string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Action act = () => ValidationHelper.ValidateFilePath(nonExistentFilePath, nameof(nonExistentFilePath));
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void ValidateDirectoryPath_WithValidDirectoryPath_ShouldNotThrow()
    {
        string existingDirectoryPath = Path.GetTempPath();

        Action act = () => ValidationHelper.ValidateDirectoryPath(existingDirectoryPath, nameof(existingDirectoryPath));
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateDirectoryPath_WithInvalidPath_ShouldThrowArgumentNullException(string path)
    {
        Action act = () => ValidationHelper.ValidateDirectoryPath(path, nameof(path));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValidateDirectoryPath_WithNonExistentDirectoryPath_ShouldThrowDirectoryNotFoundException()
    {
        string nonExistentDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Action act = () => ValidationHelper.ValidateDirectoryPath(nonExistentDirectoryPath, nameof(nonExistentDirectoryPath));
        act.Should().Throw<DirectoryNotFoundException>();
    }
}
