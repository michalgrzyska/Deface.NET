using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.System.ExternalProcessing;

[ExcludeFromCodeCoverage]
internal class ExternalProcessFactory : IExternalProcessFactory
{
    public IExternalProcess CreateExternalProcess(string path, string arguments, bool redirectStandardInput = false)
    {
        return new ExternalProcess(path, arguments, redirectStandardInput);
    }
}
