namespace Deface.NET;

/// <summary>
/// Exception representing internal Deface errors.
/// </summary>
public class DefaceException : Exception
{
    internal DefaceException(Exception ex) : base("An error occured while processing media. Check the inner exception for more details.", ex) { }
}
