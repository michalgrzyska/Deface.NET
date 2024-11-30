namespace Deface.NET.System.ExternalProcessing;

internal class ExternalProcessFactory : IExternalProcessFactory
{
    public IExternalProcess CreateExternalProcess(string path, string arguments, bool redirectStandardInput = false)
    {
        return new ExternalProcess(path, arguments, redirectStandardInput);
    }
}
