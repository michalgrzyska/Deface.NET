using System.Diagnostics;

namespace Deface.NET;

internal class ExternalProcess : IDisposable
{
    private readonly Process _process;

    public Stream InputStream => _process.StandardInput.BaseStream;
    public Stream OutputStream => _process.StandardOutput.BaseStream;

    public ExternalProcess(string path, string arguments, bool redirectStandardInput = false)
    {
        ProcessStartInfo info = new()
        {
            FileName = path,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = redirectStandardInput
        };

        _process = new()
        {
            StartInfo = info
        };
    }

    public async Task<string> ExecuteWithOutput()
    {
        Start();

        string output = await _process.StandardOutput.ReadToEndAsync();

        await WaitForExitAsync();
        return output;
    }

    public void Start() => _process.Start();
    public async Task WaitForExitAsync() => await _process.WaitForExitAsync();
    public void Dispose() => _process?.Dispose();
}
