using System.Numerics;

namespace Deface.NET.Utils;

/// <summary>
/// Author's note: I'd have to be crazy to upload the entire FluentValidation for this purpose.
/// </summary>
internal static class ValidationHelper
{
    public static void MustNotBeNull<T>(T prop, string nameOfProp) where T : class
    {
        if (prop is null)
        {
            throw new ArgumentNullException(nameOfProp);
        }
    }

    public static void MustBeGreaterOrEqualTo<T>(T prop, T value, string nameOfProp) where T : INumber<T>
    {
        if (prop < value)
        {
            throw new ArgumentOutOfRangeException(nameOfProp, $"Value must be greater or equal to {value}");
        }
    }

    public static void MustBeLessThan<T>(T prop, T value, string nameOfProp) where T : INumber<T>
    {
        if (prop >= value)
        {
            throw new ArgumentOutOfRangeException(nameOfProp, $"Value must be less than {value}");
        }
    }

    public static void MustBeLessThanOrEqualTo<T>(T prop, T value, string nameOfProp) where T : INumber<T>
    {
        if (prop > value)
        {
            throw new ArgumentOutOfRangeException(nameOfProp, $"Value must be less than or equal to {value}");
        }
    }

    public static void MustNotBeNullOrWhiteSpace(string prop, string nameOfProp)
    {
        if (string.IsNullOrWhiteSpace(prop))
        {
            throw new ArgumentNullException(nameOfProp, $"{nameOfProp} must not be null or whitespace/empty");
        }
    }

    public static void FileMustExist(string path, string nameOfProp)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"{nameOfProp} does not contain a name of existing file");
        }
    }

    public static void DirectoryMustExist(string path, string nameOfProp)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"{nameOfProp} does not contain a name of existing directory");
        }
    }

    public static void ValidateFilePath(string path, string nameOfProp)
    {
        MustNotBeNullOrWhiteSpace(path, nameOfProp);
        FileMustExist(path, nameOfProp);
    }

    public static void ValidateDirectoryPath(string path, string nameOfProp)
    {
        MustNotBeNullOrWhiteSpace(path, nameOfProp);
        DirectoryMustExist(path, nameOfProp);
    }
}
