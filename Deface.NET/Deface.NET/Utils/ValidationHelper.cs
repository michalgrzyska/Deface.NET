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
        if (prop > value)
        {
            throw new ArgumentOutOfRangeException(nameOfProp, $"Value must be less than {value}");
        }
    }

    public static void MustNotBeNullOrWhiteSpace(string prop, string nameOfProp, string prefix = "")
    {
        var message = BuildPrefix(prefix);

        if (string.IsNullOrWhiteSpace(prop))
        {
            message += $"{nameOfProp} must not be null or whitespace/empty";
            throw new ArgumentNullException(nameOfProp, message);
        }
    }

    public static void FileMustExist(string prop, string nameOfProp, string prefix = "")
    {
        var message = BuildPrefix(prefix);

        if (!File.Exists(prop))
        {
            message += $"{nameOfProp} does not contain a name of existing file";
            throw new FileNotFoundException(message);
        }
    }

    private static string BuildPrefix(string prefix)
    {
        return string.IsNullOrWhiteSpace(prefix)
            ? ""
            : $"{prefix}: ";
    }
}
