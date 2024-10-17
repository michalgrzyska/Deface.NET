namespace Deface.NET;

/// <summary>
/// Exception representing internal Deface errors.
/// </summary>
public class DefaceException : Exception
{
    internal DefaceException(string message, Exception? ex = default) : base(message, ex) { }
}
