using System.Diagnostics;

namespace Deface.NET;

internal class ExternalProcess : IDisposable
{
    private readonly Process _process;

    public Stream OutputStream => _process.StandardOutput.BaseStream;

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
