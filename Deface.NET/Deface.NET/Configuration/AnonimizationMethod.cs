using Deface.NET.Configuration;
using OpenCvSharp;

namespace Deface.NET;

/// <summary>
/// Specifies the method of object anonimization.
/// </summary>
public sealed class AnonimizationMethod
{

    internal AnonimizationType Type { get; private set; }
    internal Scalar? ColorValue { get; private set; }

    private AnonimizationMethod(AnonimizationType type, Scalar? color = default)
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
        Scalar color = new(b, g, r);
        return new(AnonimizationType.Color, color);
    }
}