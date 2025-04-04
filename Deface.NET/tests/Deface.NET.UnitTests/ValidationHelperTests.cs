﻿using Deface.NET.Common;

namespace Deface.NET.UnitTests;

public class ValidationHelperTests
{
    [Theory]
    [InlineData(5, 3)]
    [InlineData(5, 5)]
    public void MustBeGreaterOrEqualTo_WithValidValues_ShouldNotThrow(int prop, int value)
    {
        var act = () => ValidationHelper.MustBeGreaterOrEqualTo(prop, value, nameof(prop));
        act.ShouldNotThrow();
    }

    [Fact]
    public void MustBeGreaterOrEqualTo_WithInvalidValue_ShouldThrowArgumentException()
    {
        var prop = 2;
        var value = 5;

        var act = () => ValidationHelper.MustBeGreaterOrEqualTo(prop, value, nameof(prop));
        act.ShouldThrow<ArgumentException>(string.Format(ExceptionMessages.MustBeGreaterOrEqualTo, nameof(prop)));
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(5, 5)]
    public void MustBeLessThanOrEqualTo_WithValidValues_ShouldNotThrow(int prop, int value)
    {
        var act = () => ValidationHelper.MustBeLessThanOrEqualTo(prop, value, nameof(prop));
        act.ShouldNotThrow();
    }

    [Fact]
    public void MustBeLessThanOrEqualTo_WithInvalidValue_ShouldThrowArgumentException()
    {
        var prop = 7;
        var value = 5;

        var act = () => ValidationHelper.MustBeLessThanOrEqualTo(prop, value, nameof(prop));
        act.ShouldThrow<ArgumentException>(string.Format(ExceptionMessages.MustBeLessThanOrEqualTo, nameof(prop)));
    }

    [Theory]
    [InlineData("valid string")]
    [InlineData("also_valid")]
    public void MustNotBeNullOrWhiteSpace_WithValidStrings_ShouldNotThrow(string prop)
    {
        var act = () => ValidationHelper.MustNotBeNullOrWhiteSpace(prop, nameof(prop));
        act.ShouldNotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MustNotBeNullOrWhiteSpace_WithInvalidStrings_ShouldThrowArgumentException(string? prop)
    {
        var act = () => ValidationHelper.MustNotBeNullOrWhiteSpace(prop!, nameof(prop));
        act.ShouldThrow<ArgumentException>(string.Format(ExceptionMessages.MustNotBeNullOrWhiteSpace, nameof(prop)));
    }

    [Fact]
    public void FileMustExist_WithExistingFile_ShouldNotThrow()
    {
        var existingFilePath = Path.GetTempFileName();

        var act = () => ValidationHelper.FileMustExist(existingFilePath, nameof(existingFilePath));
        act.ShouldNotThrow();

        File.Delete(existingFilePath);
    }

    [Fact]
    public void FileMustExist_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        var nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.FileMustExist(nonExistentFilePath, nameof(nonExistentFilePath));
        act.ShouldThrow<FileNotFoundException>(string.Format(ExceptionMessages.FileMustExist, nameof(nonExistentFilePath)));
    }

    [Fact]
    public void DirectoryMustExist_WithExistingDirectory_ShouldNotThrow()
    {
        string existingDirectoryPath = Path.GetTempPath();

        var act = () => ValidationHelper.DirectoryMustExist(existingDirectoryPath, nameof(existingDirectoryPath));
        act.ShouldNotThrow();
    }

    [Fact]
    public void DirectoryMustExist_WithNonExistentDirectory_ShouldThrowDirectoryNotFoundException()
    {
        var nonExistentDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.DirectoryMustExist(nonExistentDirectoryPath, nameof(nonExistentDirectoryPath));
        act.ShouldThrow<DirectoryNotFoundException>(string.Format(ExceptionMessages.DirectoryMustExist, nameof(nonExistentDirectoryPath)));
    }

    [Fact]
    public void ValidateFilePath_WithValidFilePath_ShouldNotThrow()
    {
        string existingFilePath = Path.GetTempFileName();

        var act = () => ValidationHelper.ValidateFilePath(existingFilePath, nameof(existingFilePath));
        act.ShouldNotThrow();

        File.Delete(existingFilePath);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateFilePath_WithInvalidPath_ShouldThrowArgumentException(string? path)
    {
        var act = () => ValidationHelper.ValidateFilePath(path!, nameof(path));
        act.ShouldThrow<ArgumentException>(string.Format(ExceptionMessages.FileMustExist, nameof(path)));
    }

    [Fact]
    public void ValidateFilePath_WithNonExistentFilePath_ShouldThrowFileNotFoundException()
    {
        var nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.ValidateFilePath(nonExistentFilePath, nameof(nonExistentFilePath));
        act.ShouldThrow<FileNotFoundException>(string.Format(ExceptionMessages.FileMustExist, nameof(nonExistentFilePath)));
    }

    [Fact]
    public void ValidateDirectoryPath_WithValidDirectoryPath_ShouldNotThrow()
    {
        var existingDirectoryPath = Path.GetTempPath();

        var act = () => ValidationHelper.ValidateDirectoryPath(existingDirectoryPath, nameof(existingDirectoryPath));
        act.ShouldNotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateDirectoryPath_WithInvalidPath_ShouldThrowArgumentException(string? path)
    {
        var act = () => ValidationHelper.ValidateDirectoryPath(path!, nameof(path));
        act.ShouldThrow<ArgumentException>(string.Format(ExceptionMessages.DirectoryMustExist, nameof(path)));
    }

    [Fact]
    public void ValidateDirectoryPath_WithNonExistentDirectoryPath_ShouldThrowDirectoryNotFoundException()
    {
        string nonExistentDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var act = () => ValidationHelper.ValidateDirectoryPath(nonExistentDirectoryPath, nameof(nonExistentDirectoryPath));
        act.ShouldThrow<DirectoryNotFoundException>(string.Format(ExceptionMessages.DirectoryMustExist, nameof(nonExistentDirectoryPath)));
    }
}
