using System.Numerics;

namespace Deface.NET.Common;

/// <summary>
/// Author's note: I'd have to be crazy to upload the entire FluentValidation for this purpose.
/// </summary>
internal static class ValidationHelper
{
    public static void MustBeGreaterOrEqualTo<T>(T prop, T value, string nameOfProp) where T : INumber<T>
    {
        if (prop < value)
        {
            throw new ArgumentException(string.Format(ExceptionMessages.MustBeGreaterOrEqualTo, value));
        }
    }

    public static void MustBeLessThanOrEqualTo<T>(T prop, T value, string nameOfProp) where T : INumber<T>
    {
        if (prop > value)
        {
            throw new ArgumentException(string.Format(ExceptionMessages.MustBeLessThanOrEqualTo, value));
        }
    }

    public static void MustNotBeNullOrWhiteSpace(string prop, string nameOfProp)
    {
        if (string.IsNullOrWhiteSpace(prop))
        {
            throw new ArgumentException(string.Format(ExceptionMessages.MustNotBeNullOrWhiteSpace, nameOfProp));
        }
    }

    public static void FileMustExist(string path, string nameOfProp)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(string.Format(ExceptionMessages.FileMustExist, nameOfProp));
        }
    }

    public static void DirectoryMustExist(string path, string nameOfProp)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException(string.Format(ExceptionMessages.DirectoryMustExist, nameOfProp));
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
