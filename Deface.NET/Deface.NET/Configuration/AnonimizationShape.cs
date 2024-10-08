namespace Deface.NET;

/// <summary>
/// Represents possible shapes of anonimization filter.
/// </summary>
public enum AnonimizationShape
{
    /// <summary>
    /// Anonimization filter will be shaped as ellipse. Does not work with <see cref="AnonimizationMethod.Mosaic"></see>
    /// </summary>
    Ellipse = 1,

    /// <summary>
    /// Anonimization filter will be shaped as rectangle.
    /// </summary>
    Rectangle = 2
}
