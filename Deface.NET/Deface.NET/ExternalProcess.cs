using System.Diagnostics;

namespace Deface.NET;

internal class ExternalProcess : IDisposable
{
    private readonly Process process;

    public ExternalProcess(string path, string arguments)
    {
        ProcessStartInfo info = new()
        {
            FileName = path,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true
        };

        process = new()
        {
            StartInfo = info
        };
    }

    public async Task<string> ExecuteWithOutput()
    {
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();
        return output;
    }

    public void Dispose()
    {
        process.Dispose();
    }
}
