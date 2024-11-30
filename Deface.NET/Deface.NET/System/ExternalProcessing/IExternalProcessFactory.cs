namespace Deface.NET.System.ExternalProcessing;

internal interface IExternalProcessFactory
{
    IExternalProcess CreateExternalProcess(string path, string arguments, bool redirectStandardInput = false);
}
