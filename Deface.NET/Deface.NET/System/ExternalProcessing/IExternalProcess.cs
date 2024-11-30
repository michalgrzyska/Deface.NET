namespace Deface.NET.System.ExternalProcessing;

internal interface IExternalProcess : IDisposable
{
    Stream InputStream { get; }
    Stream OutputStream { get; }

    string ExecuteWithOutput();
    void Start();
    void WaitForExit();
}