using Deface.NET.Configuration;

namespace Deface.NET;

/// <summary>
/// Specifies the method of object anonimization.
/// </summary>
public readonly struct AnonimizationMethod
{
    internal AnonimizationType Type { get; private init; } = AnonimizationType.GaussianBlur;
    internal Color? ColorValue { get; private init; }

    private AnonimizationMethod(AnonimizationType type, Color? color = default)
    {
        Type = type;
        ColorValue = color;
    }

    /// <summary>
    /// Objects will be anonimized with gassian blur filter.
    /// </summary>
    public static AnonimizationMethod GaussianBlur => new(AnonimizationType.GaussianBlur);

    /// <summary>
    ///  Objects will be anonimized with mosaic filter.
    ///  <para>Important: Does not work with <see cref="AnonimizationShape.Ellipse"></see>.</para>
    /// </summary>
    public static AnonimizationMethod Mosaic => new(AnonimizationType.Mosaic);

    /// <summary>
    /// Objects will be anonimized with color filter.
    /// </summary>
    /// <param name="r">Red (0-255)</param>
    /// <param name="g">Green (0-255)</param>
    /// <param name="b">Blue (0-255)</param>
    /// <returns></returns>
    public static AnonimizationMethod Color(byte r, byte g, byte b)
    {
        Color color = new(r, g, b);
        return new(AnonimizationType.Color, color);
    }
}